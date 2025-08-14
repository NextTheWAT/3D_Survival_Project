using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Enemy : MonoBehaviour
{
    [SerializeField] private float _attackDistance;
    protected FiniteStateMachine _fsm;

    private void Awake()
    {
        _fsm = new FiniteStateMachine(null);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Update();
    }

    class AttackState : BaseState<Enemy>
    {
        public AttackState(Enemy component) : base(component) {}

        public override void End()
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            if (true)
            {
                //Component._fsm.ChangeTo(2);
            }
        }
    }
}
