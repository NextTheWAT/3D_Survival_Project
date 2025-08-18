using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy : MonoBehaviour//Character, IValueChangable
{
    [Header("Stats")]
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _waitDuration;

    [Header("Combat")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRate;
    [SerializeField] public float _attackDistance;

    [Header("AI")]
    protected NavMeshAgent _agent;
    protected FiniteStateMachine _fsm;
    protected List<IState> _states;

    [Header("Detect")]
    public Transform target;
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] private bool findPlayer;
    [SerializeField] private float targetDistance;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Test")]
    protected Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _states = new List<IState>
        {
            new WaitState(this),
            new AttackState(this),
            new MoveState(this)
        };

        _fsm = new FiniteStateMachine(_states);
    }
    void Start()
    {
        _fsm.Run();
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Update();
        Detect();
    }

    public void OnAttack()
    {
        Debug.Log("공격 성공"); //피격 판정낼게 필요
        //애니메이션 이벤트 쓸듯?
        //IValueChangable player = Component.target.gameObject.GetComponent<IValueChangable>();
        //player.ValueChanged(-Component._damage);
    }

    public void Detect()
    {
        Collider[] check = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(check.Length != 0)
        {
            target = check[0].transform;
            Vector3 targetDirection = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, targetDirection) < angle / 2)
            {
                //각도 내부에 있음
                targetDistance = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, targetDirection, targetDistance, obstacleMask)) //벽 판정
                {
                    findPlayer = true;
                }
                else
                {
                    findPlayer = false;
                }
            }
            else
            {
                //못찾음
                findPlayer = false;
            }
        }else if (findPlayer)
        {
            findPlayer = false;
        }
    }
}
