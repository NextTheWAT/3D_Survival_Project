using UnityEngine;
using UnityEngine.Events;

public class PlayerCondition : BaseCondition
{
    [Header("Player Stats")]
    [SerializeField] private Stat hunger = new Stat(100f, 0f, 100f);
    [SerializeField] private Stat stamina = new Stat(100f, 0f, 100f);

    [Header("Rates (per second)")]
    [SerializeField] private float hungerDecayPerSec = 1f;       // 허기 초당 감소
    [SerializeField] private float staminaRegenPerSec = 10f;     // 스태미나 초당 회복(달리지 않을 때)
    [SerializeField] private float healthRegenPerSec = 1f;       // 허기가 충분할 때 체력 자연 회복
    [SerializeField] private float healthStarveDamagePerSec = 2f;// 허기 0일 때 체력 감소

    [Header("Thresholds")]
    [SerializeField] private float healthRegenHungerThreshold = 60f; // 이 이상이면 체력 자연회복

    [Header("Events")]
    public UnityEvent<float, float> onHungerChanged;   // (current, normalized)
    public UnityEvent<float, float> onStaminaChanged;  // (current, normalized)

    public float Hunger => hunger.Current;
    public float Hunger01 => hunger.Normalized;
    public float Stamina => stamina.Current;
    public float Stamina01 => stamina.Normalized;

    private bool _isSprinting;

    protected override void Awake()
    {
        base.Awake();
        NotifyHunger();
        NotifyStamina();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1) 허기 자연 감소
        if (!Mathf.Approximately(hungerDecayPerSec, 0f))
        {
            hunger.Add(-hungerDecayPerSec * dt);
            NotifyHunger();
        }

        // 2) 스태미나 회복(달리지 않을 때만)
        if (!_isSprinting && stamina.Current < stamina.Max)
        {
            stamina.Add(staminaRegenPerSec * dt);
            NotifyStamina();
        }

        // 3) 허기 상태에 따른 체력 변화
        if (Mathf.Approximately(hunger.Current, hunger.Min))
        {
            // 굶주림 데미지
            if (!Mathf.Approximately(healthStarveDamagePerSec, 0f))
                AddHealth(-healthStarveDamagePerSec * dt);
        }
        else if (hunger.Current >= healthRegenHungerThreshold && Health < health.Max)
        {
            // 허기가 충분하면 체력 자연 회복
            if (!Mathf.Approximately(healthRegenPerSec, 0f))
                AddHealth(healthRegenPerSec * dt);
        }
    }

    // ===== 외부 API (게임 로직에서 호출) =====
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        AddHealth(-amount);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        AddHealth(amount);
    }

    public bool TryUseStamina(float amount)
    {
        if (amount <= 0f) return true;
        if (stamina.Current < amount) return false;

        stamina.Add(-amount);
        NotifyStamina();
        return true;
    }

    public void SetSprinting(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    public void RestoreStamina(float amount)
    {
        if (amount <= 0f) return;
        stamina.Add(amount);
        NotifyStamina();
    }

    public void Eat(float hungerGain, float healAmount = 0f)
    {
        if (hungerGain > 0f)
        {
            hunger.Add(hungerGain);
            NotifyHunger();
        }
        if (healAmount > 0f) Heal(healAmount);
    }

    // ===== 이벤트 알림 =====
    private void NotifyHunger()
    {
        onHungerChanged?.Invoke(hunger.Current, hunger.Normalized);
    }

    private void NotifyStamina()
    {
        onStaminaChanged?.Invoke(stamina.Current, stamina.Normalized);
    }

    protected override void Die()
    {
        base.Die();
        // TODO: 사망 처리(리스폰/게임오버 등)
    }
}
