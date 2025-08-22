using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Enemy : BaseCondition, IValueChangable
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
    private int _attackCurrentCombo;

    [Header("HitBox")]
    public Transform[] hitbox;
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
    private bool findPlayer;
    private float targetDistance;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Animation")]
    private bool _isDead;

    [Header("Test")]
    protected Animator _anim;
    private EnemySpawnArea _spawnOwner;
    private int _spawnOwnerId;

    public event Action damageFlash;

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
    private void Update()
    {
        if (_isDead) return; // 죽은 뒤엔 로직 중지
        _fsm.Update();
        Detect();
    }

    private void FixedUpdate()
    {
        if (_isDead) return;
        if (_isAttackActive) CheckOverlap();
    }

    public void SetOwner(EnemySpawnArea owner,int id)
    {
        _spawnOwner = owner;
        _spawnOwnerId = id;
    }

    protected override void Die()
    {
        if (_isDead) return;
        _isDead = true;

        // 1) 애니메이터 Death 파라미터 ON (해시 사용)
        if (_anim)
        {
            // Death가 Bool인지 Trigger인지 컨트롤러에 따라 선택:
            // Bool이면:
            _anim.SetBool(EnemyAnimParam.Death, true);
            // Trigger면:
            // _anim.SetTrigger(EnemyAnimParam.Death);
        }

        // 2) 공격/AI/이동 정지
        _isAttackActive = false;
        _hittedTarget.Clear();

        if (_agent)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            // 루트모션만 쓰고 싶으면:
            // _agent.enabled = false;
        }

        // 3) FSM 전환/스폰 알림
        if (_spawnOwner != null)
            _spawnOwner.MonsterDeath(_spawnOwnerId, this.gameObject);

        _fsm.ChangeTo(3); // DeathState index 사용 중이면 유지(가독성 위해 enum으로 바꾸는 걸 추천)
        // base.Die();  // onDied 이벤트를 쓰면 유지, 아니면 생략 가능
    }

    public override float ValueChanged(int amount)
    {
        if (_isDead) return health;   // 사망 후 무시(선택)
        damageFlash?.Invoke();
        AddHealth(amount);            // 음수면 감소
        return health;                // 변경 후 체력 반환
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
        Collider[] hitTarget = Physics.OverlapBox(hitbox[_attackCurrentCombo].position, hitboxSize/2, hitbox[_attackCurrentCombo].rotation, targetMask);
        Debug.Log("OverlapBox 감지된 타겟 수: " + hitTarget.Length);
        if (hitTarget.Length != 0)
        {
            if (!_hittedTarget.Contains(hitTarget[0]))
            {
                if(hitTarget[0].TryGetComponent<IValueChangable>(out IValueChangable target))
                {
                    target.ValueChanged(-_damage);
                }
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
