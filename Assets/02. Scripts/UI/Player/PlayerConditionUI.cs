using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    [Header("Target (Logic)")]
    [SerializeField] private PlayerCondition target; // 인스펙터에서 Player 오브젝트의 PlayerCondition 참조

    [System.Serializable]
    public class Bar
    {
        public Image fillImage;          // Filled Image
        public RectTransform widthBar;   // 폭 조절할 RectTransform 추가
        public float widthBarMax = 200f;
    }

    [Header("Bars")]
    [SerializeField] private Bar health;
    [SerializeField] private Bar hunger;
    [SerializeField] private Bar stamina;

    private void OnEnable()
    {
        if (!target)
        {
            target = FindObjectOfType<PlayerCondition>();
        }

        // 이벤트 구독
        target.onHealthChanged.AddListener(OnHealthChanged);
        target.onHungerChanged.AddListener(OnHungerChanged);
        target.onStaminaChanged.AddListener(OnStaminaChanged);

        // 초기 반영
        OnHealthChanged(target.Health, target.Health01);
        OnHungerChanged(target.Hunger, target.Hunger01);
        OnStaminaChanged(target.Stamina, target.Stamina01);
    }

    private void OnDisable()
    {
        if (!target) return;
        target.onHealthChanged.RemoveListener(OnHealthChanged);
        target.onHungerChanged.RemoveListener(OnHungerChanged);
        target.onStaminaChanged.RemoveListener(OnStaminaChanged);
    }

    // ===== 이벤트 핸들러 =====
    private void OnHealthChanged(float current, float normalized) => SetBar(health, normalized);
    private void OnHungerChanged(float current, float normalized) => SetBar(hunger, normalized);
    private void OnStaminaChanged(float current, float normalized) => SetBar(stamina, normalized);

    // ===== 공통 적용 =====
    private static void SetBar(Bar bar, float v01)
    {
        v01 = Mathf.Clamp01(v01);

        if (bar.widthBar)
        {
            var size = bar.widthBar.sizeDelta;
            size.x = bar.widthBarMax * v01;
            bar.widthBar.sizeDelta = size;
        }

    }
}
