using Object.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Input;

[RequireComponent(typeof(EquipmentController))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("Animator Trigger")]
    [SerializeField] private string attackTriggerName = "Attack";

    [Header("Ray Hit")]
    [SerializeField] private bool useRayHit = true;
    [SerializeField] private Transform rayOrigin;           // 비우면 카메라 리그/메인카메라/자기 transform 순
    [SerializeField] private float rayRange = 2.5f;
    [SerializeField] private LayerMask damageMask;          // 비어 있으면 전체(~0)로 처리
    [SerializeField] private int rayDamage = 10;
    [SerializeField] private float attackCooldown = 0f;

    private EquipmentController _equip;
    private CharacterControls _controls;
    private PlayerPerspectiveController _perspective;       // 카메라 리그 사용
    private float _lastAttackTime;

    private void Awake()
    {
        _equip = GetComponent<EquipmentController>();
        _controls = new CharacterControls();
        _perspective = GetComponent<PlayerPerspectiveController>(); //
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        _controls.Player.Attack.performed -= OnAttack;
        _controls.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (_equip == null || !_equip.HasItem)
            return;

        if (attackCooldown > 0f && Time.time - _lastAttackTime < attackCooldown) return;

        // 1) 장착 아이템 애니메이션 트리거
        var anim = _equip.CurrentItemAnimator;
        if (anim)
        {
            anim.ResetTrigger(attackTriggerName);
            anim.SetTrigger(attackTriggerName);
        }

        // 2) 레이 히트 (원래 쓰던 방식 고정)
        if (useRayHit) TryRayHit();

        _lastAttackTime = Time.time;
    }

    private void TryRayHit()
    {
        // 원래 상호작용과 동일한 기준: 카메라 리그 → 메인카메라 → 자기 transform
        Transform originT = rayOrigin
                            ? rayOrigin
                            : (_perspective ? _perspective.PerspectiveCameraRig
                                            : (Camera.main ? Camera.main.transform : transform));
        Vector3 origin = originT.position + originT.forward * 0.1f; // 자기 콜라이더 피하기
        Vector3 dir = originT.forward;

        // 마스크 미지정이면 전체 레이어 대상으로
        int mask = (damageMask.value == 0) ? ~0 : damageMask.value;

        // 근접 관용성 높이기: SphereCast(+ 트리거도 맞춤)
        if (Physics.SphereCast(origin, 0.2f, dir, out var hit, rayRange, mask, QueryTriggerInteraction.Collide))
        {
            // 자기 자신/자식은 무시
            if (hit.collider.transform.IsChildOf(transform)) return;

            // 1) IDamagable 우선
            if (hit.collider.TryGetComponent<IDamagable>(out var dmg))
            {
                dmg.TakePhysicalDamage(rayDamage);
                return;
            }
            // 2) IValueChangable (Enemy가 이 인터페이스일 수 있음)
            if (hit.collider.TryGetComponent<IValueChangable>(out var val))
            {
                val.ValueChanged(-rayDamage);
                return;
            }
        }

#if UNITY_EDITOR
        Debug.DrawRay(origin, dir * rayRange, Color.red, 0.25f);
#endif
    }
}
