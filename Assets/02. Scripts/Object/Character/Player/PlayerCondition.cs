using UnityEngine;
using UnityEngine.Events;

public class PlayerCondition : BaseCondition, IDamagable
{
    [Header("Hunger")]
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float minHunger = 0f;
    [SerializeField] private float maxHunger = 100f;

    [Header("Stamina")]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float minStamina = 0f;
    [SerializeField] private float maxStamina = 100f;

    [Header("Rates (per second)")]
    [SerializeField] private float hungerDecayPerSec = 1f;        // 허기 초당 감소
    [SerializeField] private float staminaRegenPerSec = 10f;      // (비스프린트) 초당 회복
    [SerializeField] private float healthRegenPerSec = 1f;        // 허기 충분시 체력 자연회복
    [SerializeField] private float healthStarveDamagePerSec = 2f; // 허기 0일 때 초당 체력 감소

    [Header("Thresholds")]
    [SerializeField] private float healthRegenHungerThreshold = 60f;

    [Header("Events")]
    public UnityEvent<float, float> onHungerChanged;   // (current, normalized)
    public UnityEvent<float, float> onStaminaChanged;  // (current, normalized)

    public float Hunger => hunger;
    public float Hunger01 => Normalize01(hunger, minHunger, maxHunger);
    public float Stamina => stamina;
    public float Stamina01 => Normalize01(stamina, minStamina, maxStamina);

    private bool _isSprinting;

    protected override void Awake()
    {
        base.Awake();
        // 초기값을 UI에 바로 반영
        SetHunger(hunger);
        SetStamina(stamina);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1) 허기 자연 감소
        if (!Mathf.Approximately(hungerDecayPerSec, 0f))
            SetHunger(hunger - hungerDecayPerSec * dt);

        // 2) 스태미나 회복(달리지 않을 때)
        if (!_isSprinting && stamina < maxStamina)
            SetStamina(stamina + staminaRegenPerSec * dt);

        // 3) 허기 상태에 따른 체력 변화
        if (Mathf.Approximately(hunger, minHunger))
        {
            if (!Mathf.Approximately(healthStarveDamagePerSec, 0f))
                AddHealth(-healthStarveDamagePerSec * dt);
        }
        else if (hunger >= healthRegenHungerThreshold && Health < maxHealth)
        {
            if (!Mathf.Approximately(healthRegenPerSec, 0f))
                AddHealth(healthRegenPerSec * dt);
        }
    }

    // ====== 외부에서 쓰는 간단 API ======
    public void TakePhysicalDamage(int damageAmount)   // IDamagable 규약
    {
        if (damageAmount <= 0) return;
        AddHealth(-damageAmount);
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        AddHealth(amount);
    }

    public bool TryUseStamina(float amount)
    {
        if (amount <= 0f) return true;
        if (stamina < amount) return false;

        SetStamina(stamina - amount);
        return true;
    }

    public void RestoreStamina(float amount)
    {
        if (amount <= 0f) return;
        SetStamina(stamina + amount);
    }

    public void Eat(float hungerGain, float healAmount = 0f)
    {
        if (hungerGain > 0f) SetHunger(hunger + hungerGain);
        if (healAmount > 0f) Heal(healAmount);
    }

    public void SetSprinting(bool isSprinting) => _isSprinting = isSprinting;

    // ====== 내부 헬퍼 & 이벤트 알림 ======
    private static float Normalize01(float v, float min, float max)
    {
        if (max <= min) return 0f;
        return Mathf.Clamp01((v - min) / (max - min));
    }

    private void SetHunger(float v)
    {
        hunger = Mathf.Clamp(v, minHunger, maxHunger);
        onHungerChanged?.Invoke(hunger, Hunger01);
    }

    private void SetStamina(float v)
    {
        stamina = Mathf.Clamp(v, minStamina, maxStamina);
        onStaminaChanged?.Invoke(stamina, Stamina01);
    }
}
