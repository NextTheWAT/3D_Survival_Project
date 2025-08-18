using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// 공통 UI 베이스: DOTween으로 Open/Close 애니메이션 처리
[DisallowMultipleComponent]
public abstract class BaseUI : MonoBehaviour
{
    // 어떤 애니메이션을 사용할지 결정하는 열거형
    public enum AnimType { None, Fade, Scale }

    [Header("Animation")]
    [SerializeField] private AnimType animType = AnimType.Fade;   // 애니메이션 타입
    [SerializeField, Min(0f)] private float duration = 0.25f;     // 애니메이션 재생 시간
    [SerializeField] private float scaleFactor = 0f;              // Scale 애니메이션 시 최종 크기 (닫힐 때 사용)
    [SerializeField] private Ease ease = Ease.Linear;             // DOTween 이징 함수
    [Tooltip("Time.timeScale=0 이어도 애니메이션 동작")]
    [SerializeField] private bool ignoreTimeScale = true;         // 일시정지 중에도 애니메이션 동작 여부

    [Header("Start State")]
    [SerializeField] private bool startHidden = true;             // 시작 시 숨겨둘지 여부

    [Header("Events")]
    public UnityEvent OnOpened;   // 열림 완료 이벤트
    public UnityEvent OnClosed;   // 닫힘 완료 이벤트

    // UI 요소 캐싱
    protected CanvasGroup canvasGroup;
    protected RectTransform rect;
    protected bool isOpen;        // 현재 열림 상태인지 여부

    Sequence seq;                 // DOTween 시퀀스
    Vector2 cachedAnchoredPos;    // 원래 anchoredPosition 저장

    protected virtual void Awake()
    {
        rect = (RectTransform)transform;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        cachedAnchoredPos = rect.anchoredPosition;

        // 시작 상태 세팅
        if (startHidden)
        {
            ApplyHiddenImmediate();
            gameObject.SetActive(false);
            isOpen = false;
        }
        else
        {
            ApplyShownImmediate();
            isOpen = true;
        }
    }

    protected virtual void OnDisable()
    {
        KillTween(); // 꺼질 때 애니메이션 중단
    }

    // UI 열기
    public virtual void Open()
    {
        if (isOpen) return;   // 이미 열려있으면 무시
        KillTween();

        gameObject.SetActive(true);
        PrepareForOpen();     // 열기 전 초기 세팅

        // 애니메이션 동안 UI 클릭 불가 처리
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        // DOTween 시퀀스 시작
        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendOpenTween(seq); // 열림 애니메이션 추가
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = true;
               canvasGroup.blocksRaycasts = true;
               canvasGroup.interactable = true;
               OnOpened?.Invoke(); // 이벤트 발동
           });
    }

    // UI 닫기
    public virtual void Close()
    {
        if (!isOpen) return;  // 이미 닫혀있으면 무시
        KillTween();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendCloseTween(seq); // 닫힘 애니메이션 추가
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = false;
               gameObject.SetActive(false);
               OnClosed?.Invoke(); // 이벤트 발동
           });
    }

    // 자식 클래스에서 필요 시 텍스트, 버튼 비활성화 (필요할때 활성화시키기 위해서)
    public virtual void UnActive() { }

    // 현재 실행 중인 애니메이션 정리
    void KillTween()
    {
        if (seq != null && seq.IsActive()) seq.Kill();
        seq = null;
    }

    // Open 전에 상태 준비
    void PrepareForOpen()
    {
        switch (animType)
        {
            case AnimType.None:
                ApplyShownImmediate();
                break;
            case AnimType.Fade:
                canvasGroup.alpha = 0f;
                rect.anchoredPosition = cachedAnchoredPos;
                break;
            case AnimType.Scale:
                canvasGroup.alpha = 1f;
                rect.localScale = Vector3.one;
                rect.anchoredPosition = cachedAnchoredPos;
                break;
        }
    }

    // Open 애니메이션 추가
    void AppendOpenTween(Sequence s)
    {
        switch (animType)
        {
            case AnimType.None:
                ApplyShownImmediate();
                break;
            case AnimType.Fade:
                s.Append(canvasGroup.DOFade(1f, duration));
                break;
            case AnimType.Scale:
                s.Append(rect.DOScale(1f, duration));
                break;
        }
    }

    // Close 애니메이션 추가
    void AppendCloseTween(Sequence s)
    {
        switch (animType)
        {
            case AnimType.None:
                ApplyHiddenImmediate();
                break;
            case AnimType.Fade:
                s.Append(canvasGroup.DOFade(0f, duration));
                break;
            case AnimType.Scale:
                s.Append(rect.DOScale(scaleFactor, duration));
                break;
        }
    }

    // 즉시 보여주기 (애니메이션 없이)
    void ApplyShownImmediate()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // 즉시 숨기기 (애니메이션 없이)
    void ApplyHiddenImmediate()
    {
        canvasGroup.alpha = 0f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
