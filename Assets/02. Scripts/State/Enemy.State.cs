using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy
{
    
    class AttackState : BaseState<Enemy>
    {
        public AttackState(Enemy component) : base(component) { }

        public override void End()
        {
            Debug.Log("attack 끝");
            Component._agent.speed = Component._walkSpeed;
            Component._agent.isStopped = true;
        }

        public override void Start()
        {
            Debug.Log("attack 진입");
            Component._agent.speed = Component._runSpeed;
            Component._agent.isStopped = false;
        }

        public override void Update()
        {
            Debug.Log("attack 상태");
            if (Component.findPlayer)
            {
                //Component.transform.LookAt(LookTarget());
                //적이 시야 범위 내에 있을 경우
                if(Component.targetDistance > Component._attackDistance)
                {
                    Component._agent.isStopped = false;
                    Component._agent.SetDestination(Component.target.position);
                }
                else
                {
                    Component._agent.isStopped = true;
                    Debug.Log("공격 (딜레이 필요)");
                }
            }
            else
            {
                //적이 범위에서 벗어났을 시
                Component._fsm.ChangeTo(0);
            }
        }

        public Vector3 LookTarget()
        {
            return new Vector3(Component.target.position.x, 0, Component.target.position.z);
        }
    }

    class WaitState : BaseState<Enemy>
    {
        public WaitState(Enemy component) : base(component) { }

        public override void End()
        {
            Debug.Log("wait 끝");
            Component._agent.speed = Component._walkSpeed;
            Component._agent.isStopped = true;
        }

        public override void Start()
        {
            Debug.Log("wait 진입");
            Component._agent.isStopped = false;
        }

        public override void Update()
        {
            Debug.Log("wait 상태");
            
            if (Component.findPlayer) {
                //적 발견
                Component._fsm.ChangeTo(1);
            }
            else
            {
                //적 발견 x 이동
                Component._fsm.ChangeTo(2);
            }
            
        }
    }

    class MoveState : BaseState<Enemy>
    {
        public MoveState(Enemy component) : base(component) { }

        public override void End()
        {
            Debug.Log("Move 끝");
            Component._agent.speed = Component._walkSpeed;
            Component._agent.isStopped = true;
        }

        public override void Start()
        {
            Debug.Log("Move 진입");
            Component._agent.isStopped = false;
            Component._agent.SetDestination(MovePosition());
        }

        public override void Update()
        {
            Debug.Log("Move 상태");
            
            if (Component.findPlayer)
            {
                Component._fsm.ChangeTo(1);
                return;
            }

            //도착지점 도착
            if (Component._agent.remainingDistance < Component._agent.stoppingDistance + 0.1f)
            {
                Component._fsm.ChangeTo(0);
            }
        }

        Vector3 MovePosition()
        {
            NavMeshHit hit;

            NavMesh.SamplePosition(Component.transform.position + (Random.onUnitSphere * 5f), out hit, Component._moveDistance, NavMesh.AllAreas);

            return hit.position;
        }
    }
}
