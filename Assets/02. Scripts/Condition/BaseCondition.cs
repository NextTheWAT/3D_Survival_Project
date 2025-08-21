using UnityEngine;
using UnityEngine.Events;

public abstract class BaseCondition : MonoBehaviour, IValueChangable
{
    [Header("Health")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float minHealth = 0f;
    [SerializeField] protected float maxHealth = 100f;

    [Header("Events")]
    public UnityEvent<float, float> onHealthChanged; // (current, normalized)
    public UnityEvent onDied;

    public float Health => health;
    public float Health01 => Mathf.Approximately(maxHealth - minHealth, 0f)
        ? 0f
        : Mathf.Clamp01((health - minHealth) / (maxHealth - minHealth));

    protected virtual void Awake()
    {
        NotifyHealth();
    }

    protected void NotifyHealth()
    {
        onHealthChanged?.Invoke(health, Health01);
    }

    public virtual void SetHealth(float value)
    {
        health = Mathf.Clamp(value, minHealth, maxHealth);
        NotifyHealth();
        if (Mathf.Approximately(health, minHealth)) Die();
    }

    public virtual void AddHealth(float delta)
    {
        SetHealth(health + delta);
    }

    // IValueChangable 구현 (int 양/음수로 증감)  ← 인터페이스 시그니처 맞춤
    public abstract float ValueChanged(int amount);   // returns current after change

    protected virtual void Die()
    {
        onDied?.Invoke();
        // 파생 클래스에서 추가 처리
    }
}
