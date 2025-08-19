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

    [Header("HitBox")]
    public Transform hitbox;
    public Vector3 hitboxSize;
    private bool _isAttackActive = false;
    [SerializeField] private List<Collider> _hittedTarget = new();

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
            new MoveState(this),
            new DeathState(this)
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

    private void FixedUpdate()
    {
        if (!_isAttackActive) {
            return;
        }

        CheckOverlap();
    }

    public void OnEnableAttack()
    {
        _isAttackActive = true;
        _hittedTarget.Clear();
        Debug.Log("hitbox 활성화");
    }

    public void OnDisableAttack()
    {
        _isAttackActive = false;
        Debug.Log("hitbox 비활성화");
    }

    public void CheckOverlap()
    {
        Collider[] hitTarget = Physics.OverlapBox(hitbox.position, hitboxSize/2, hitbox.rotation, targetMask);
        Debug.Log("OverlapBox 감지된 타겟 수: " + hitTarget.Length);
        if (hitTarget.Length != 0)
        {
            if (!_hittedTarget.Contains(hitTarget[0]))
            {
                Debug.Log(hitTarget[0].name + "데미지 주기"); //데미지 주는 로직 작성
                _hittedTarget.Add(hitTarget[0]);
            }
        }
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
