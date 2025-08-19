using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy
{
    
    class AttackState : BaseState<Enemy>
    {
        public AttackState(Enemy component) : base(component) { }
        private float waitTimer = 0f;

        public override void End()
        {
            Debug.Log("attack 끝");
            Component._agent.speed = Component._walkSpeed;
            Component._agent.isStopped = true;
            Component._anim.SetBool(EnemyAnimParam.Run, false);
        }

        public override void Start()
        {
            Debug.Log("attack 진입");
            Component._agent.speed = Component._runSpeed;
            Component._agent.isStopped = false;
            Component._anim.SetBool(EnemyAnimParam.Run, true);
        }

        public override void Update()
        {
            Debug.Log("attack 상태");
            waitTimer += Time.deltaTime;
            if (Component.findPlayer)
            {
                //적이 시야 범위 내에 있을 경우
                if(Component.targetDistance > Component._attackDistance)
                {
                    Component._agent.isStopped = false;
                    Component._agent.SetDestination(Component.target.position);
                    Component._anim.SetBool(EnemyAnimParam.Run, true);
                }
                else
                {
                    Component._agent.isStopped = true;
                    Component._anim.SetBool(EnemyAnimParam.Run, false);

                    if (waitTimer > Component._attackRate)
                    {
                        Component._anim.SetTrigger(EnemyAnimParam.Attack);
                        waitTimer = 0f;
                    }
                }
            }
            else
            {
                //적이 범위에서 벗어났을 시
                Component._fsm.ChangeTo(0);
            }
        }
    }

    class WaitState : BaseState<Enemy>
    {
        public WaitState(Enemy component) : base(component) { }

        private float waitTimer = 0f;

        public override void End()
        {
            Debug.Log("wait 끝");
            Component._agent.speed = Component._walkSpeed;
            Component._agent.isStopped = true;
            Component._anim.SetBool(EnemyAnimParam.Idle, false);
        }

        public override void Start()
        {
            Debug.Log("wait 진입");
            waitTimer = 0f;
            Component._agent.isStopped = false;
            Component._anim.SetBool(EnemyAnimParam.Idle, true);
        }

        public override void Update()
        {
            Debug.Log("wait 상태");
            
            if (Component.findPlayer) {
                //적 발견
                Component._fsm.ChangeTo(1);
                return;
            }

            waitTimer += Time.deltaTime;

            if (waitTimer >= Component._waitDuration)
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
            Component._anim.SetBool(EnemyAnimParam.Walk, false);
        }

        public override void Start()
        {
            Debug.Log("Move 진입");
            Component._agent.isStopped = false;
            Component._agent.SetDestination(MovePosition());
            Component._anim.SetBool(EnemyAnimParam.Walk, true);
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

    class DeathState : BaseState<Enemy>
    {
        public DeathState(Enemy component) : base(component){}

        public override void End()
        {
            
        }

        public override void Start()
        {
            Component._agent.isStopped = true;
            Component._agent.ResetPath();
            Component._agent.enabled = false;

            Component._anim.SetBool(EnemyAnimParam.Death, true);
        }

        public override void Update()
        {
            
        }
    }
}
