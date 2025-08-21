using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// ���� UI ���̽�: DOTween���� Open/Close �ִϸ��̼� ó��
[DisallowMultipleComponent]
public abstract class BaseUI : MonoBehaviour
{
    public enum AnimType { None, Fade, SlideFromTop, SlideFromBottom, SlideFromLeft, SlideFromRight }

    [Header("Animation")]
    [SerializeField] private AnimType animType = AnimType.Fade;
    [SerializeField, Min(0f)] private float duration = 0.25f;
    [SerializeField] private Ease ease = Ease.OutCubic;
    [Tooltip("Time.timeScale=0 �̾ �ִϸ��̼� ����")]
    [SerializeField] private bool ignoreTimeScale = true;
    [Tooltip("�����̵� �� �̵� �Ÿ�(0�̸� �ڵ����� rect ũ�� ���)")]
    [SerializeField] private float slideDistance = 0f;

    [Header("Start State")]
    [SerializeField] private bool startHidden = true;

    //[Header("Events")]
    //public UnityEvent OnOpenStarted;
    //public UnityEvent OnOpened;
    //public UnityEvent OnCloseStarted;
    //public UnityEvent OnClosed;

    protected CanvasGroup canvasGroup;
    protected RectTransform rect;
    protected bool isOpen;

    Sequence seq;
    Vector2 cachedAnchoredPos;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogError($"[BaseUI] {name}���� RectTransform�� �ʿ��մϴ�. Canvas ���� UI�� ��ġ�ϼ���.");
            rect = gameObject.AddComponent<RectTransform>(); // �ӽ� ����(�����ϸ� Canvas �Ʒ��� �ű��)
        }

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        cachedAnchoredPos = rect.anchoredPosition;

        // �ʱ� ���� ����
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
        KillTween();
    }

    public virtual void Open()
    {
        if (isOpen) return;
        KillTween();

        gameObject.SetActive(true);
        PrepareForOpen();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        //OnOpenStarted?.Invoke();

        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendOpenTween(seq);
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = true;
               canvasGroup.blocksRaycasts = true;
               canvasGroup.interactable = true;
               //OnOpened?.Invoke();
           });
    }

    public virtual void Close()
    {
        if (!isOpen) return;
        KillTween();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        //OnCloseStarted?.Invoke();

        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendCloseTween(seq);
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = false;
               gameObject.SetActive(false);
               //OnClosed?.Invoke();
           });
    }

    public virtual void UnActive()
    {

    }


    void KillTween()
    {
        if (seq != null && seq.IsActive()) seq.Kill();
        seq = null;
    }

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

            case AnimType.SlideFromTop:
            case AnimType.SlideFromBottom:
            case AnimType.SlideFromLeft:
            case AnimType.SlideFromRight:
                canvasGroup.alpha = 1f;
                rect.anchoredPosition = cachedAnchoredPos + GetSlideOffset(animType, true);
                break;
        }
    }

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

            case AnimType.SlideFromTop:
            case AnimType.SlideFromBottom:
            case AnimType.SlideFromLeft:
            case AnimType.SlideFromRight:
                s.Append(rect.DOAnchorPos(cachedAnchoredPos, duration));
                break;
        }
    }

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

            case AnimType.SlideFromTop:
            case AnimType.SlideFromBottom:
            case AnimType.SlideFromLeft:
            case AnimType.SlideFromRight:
                s.Append(rect.DOAnchorPos(cachedAnchoredPos + GetSlideOffset(animType, false), duration));
                break;
        }
    }

    Vector2 GetSlideOffset(AnimType type, bool forOpenPhase)
    {
        float d = slideDistance;
        if (Mathf.Approximately(d, 0f))
        {
            // �ڵ� �Ÿ�: rect ũ�� + 100 ����
            Vector2 size = rect.rect.size;
            d = (type == AnimType.SlideFromLeft || type == AnimType.SlideFromRight)
                ? size.x + 100f
                : size.y + 100f;
        }

        return type switch
        {
            AnimType.SlideFromTop => new Vector2(0, +d),
            AnimType.SlideFromBottom => new Vector2(0, -d),
            AnimType.SlideFromLeft => new Vector2(-d, 0),
            AnimType.SlideFromRight => new Vector2(+d, 0),
            _ => Vector2.zero
        };
    }

    void ApplyShownImmediate()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    void ApplyHiddenImmediate()
    {
        canvasGroup.alpha = 0f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
