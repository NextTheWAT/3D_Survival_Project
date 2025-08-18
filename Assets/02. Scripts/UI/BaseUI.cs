using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// ���� UI ���̽�: DOTween���� Open/Close �ִϸ��̼� ó��
[DisallowMultipleComponent]
public abstract class BaseUI : MonoBehaviour
{
    // � �ִϸ��̼��� ������� �����ϴ� ������
    public enum AnimType { None, Fade, Scale }

    [Header("Animation")]
    [SerializeField] private AnimType animType = AnimType.Fade;   // �ִϸ��̼� Ÿ��
    [SerializeField, Min(0f)] private float duration = 0.25f;     // �ִϸ��̼� ��� �ð�
    [SerializeField] private float scaleFactor = 0f;              // Scale �ִϸ��̼� �� ���� ũ�� (���� �� ���)
    [SerializeField] private Ease ease = Ease.Linear;             // DOTween ��¡ �Լ�
    [Tooltip("Time.timeScale=0 �̾ �ִϸ��̼� ����")]
    [SerializeField] private bool ignoreTimeScale = true;         // �Ͻ����� �߿��� �ִϸ��̼� ���� ����

    [Header("Start State")]
    [SerializeField] private bool startHidden = true;             // ���� �� ���ܵ��� ����

    [Header("Events")]
    public UnityEvent OnOpened;   // ���� �Ϸ� �̺�Ʈ
    public UnityEvent OnClosed;   // ���� �Ϸ� �̺�Ʈ

    // UI ��� ĳ��
    protected CanvasGroup canvasGroup;
    protected RectTransform rect;
    protected bool isOpen;        // ���� ���� �������� ����

    Sequence seq;                 // DOTween ������
    Vector2 cachedAnchoredPos;    // ���� anchoredPosition ����

    protected virtual void Awake()
    {
        rect = (RectTransform)transform;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        cachedAnchoredPos = rect.anchoredPosition;

        // ���� ���� ����
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
        KillTween(); // ���� �� �ִϸ��̼� �ߴ�
    }

    // UI ����
    public virtual void Open()
    {
        if (isOpen) return;   // �̹� ���������� ����
        KillTween();

        gameObject.SetActive(true);
        PrepareForOpen();     // ���� �� �ʱ� ����

        // �ִϸ��̼� ���� UI Ŭ�� �Ұ� ó��
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        // DOTween ������ ����
        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendOpenTween(seq); // ���� �ִϸ��̼� �߰�
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = true;
               canvasGroup.blocksRaycasts = true;
               canvasGroup.interactable = true;
               OnOpened?.Invoke(); // �̺�Ʈ �ߵ�
           });
    }

    // UI �ݱ�
    public virtual void Close()
    {
        if (!isOpen) return;  // �̹� ���������� ����
        KillTween();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        seq = DOTween.Sequence().SetUpdate(ignoreTimeScale);
        AppendCloseTween(seq); // ���� �ִϸ��̼� �߰�
        seq.SetEase(ease)
           .OnComplete(() =>
           {
               isOpen = false;
               gameObject.SetActive(false);
               OnClosed?.Invoke(); // �̺�Ʈ �ߵ�
           });
    }

    // �ڽ� Ŭ�������� �ʿ� �� �ؽ�Ʈ, ��ư ��Ȱ��ȭ (�ʿ��Ҷ� Ȱ��ȭ��Ű�� ���ؼ�)
    public virtual void UnActive() { }

    // ���� ���� ���� �ִϸ��̼� ����
    void KillTween()
    {
        if (seq != null && seq.IsActive()) seq.Kill();
        seq = null;
    }

    // Open ���� ���� �غ�
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

    // Open �ִϸ��̼� �߰�
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

    // Close �ִϸ��̼� �߰�
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

    // ��� �����ֱ� (�ִϸ��̼� ����)
    void ApplyShownImmediate()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // ��� ����� (�ִϸ��̼� ����)
    void ApplyHiddenImmediate()
    {
        canvasGroup.alpha = 0f;
        rect.localScale = Vector3.one;
        rect.anchoredPosition = cachedAnchoredPos;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
