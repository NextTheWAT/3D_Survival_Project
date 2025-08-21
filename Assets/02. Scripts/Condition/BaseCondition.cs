using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Stat
{
    [SerializeField] private float current;
    [SerializeField] private float min;
    [SerializeField] private float max;

    public float Current => current;
    public float Min => min;
    public float Max => max;
    public float Normalized => Mathf.Approximately(max - min, 0f) ? 0f : (current - min) / (max - min);

    public Stat(float current, float min, float max)
    {
        this.min = min;
        this.max = Mathf.Max(min, max);
        this.current = Mathf.Clamp(current, min, this.max);
    }

    public float Set(float value)
    {
        current = Mathf.Clamp(value, min, max);
        return current;
    }

    public float Add(float delta)
    {
        current = Mathf.Clamp(current + delta, min, max);
        return current;
    }

    public void SetBounds(float newMin, float newMax, bool clampToNewBounds = true)
    {
        min = newMin;
        max = Mathf.Max(newMin, newMax);
        if (clampToNewBounds) current = Mathf.Clamp(current, min, max);
    }
}

public class BaseCondition : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected Stat health = new Stat(100f, 0f, 100f);

    [Header("Events")]
    public UnityEvent<float, float> onHealthChanged; // (current, normalized)
    public UnityEvent onDied;

    public float Health => health.Current;
    public float Health01 => health.Normalized;

    protected virtual void Awake()
    {
        // 필요 시 초기화
        NotifyHealth();
    }

    protected void NotifyHealth()
    {
        onHealthChanged?.Invoke(health.Current, health.Normalized);
    }

    public virtual void SetHealth(float value)
    {
        health.Set(value);
        NotifyHealth();
        if (Mathf.Approximately(health.Current, health.Min)) Die();
    }

    public virtual void AddHealth(float delta)
    {
        health.Add(delta);
        NotifyHealth();
        if (Mathf.Approximately(health.Current, health.Min)) Die();
    }

    protected virtual void Die()
    {
        onDied?.Invoke();
        // 공통 사망 처리 훅 (파생 클래스에서 override)
    }
}
