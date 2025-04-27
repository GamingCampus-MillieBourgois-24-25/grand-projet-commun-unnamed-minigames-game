using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using AxoLoop.Minigames.MatchTheStars;

public class MTSDragAndDropBehavior : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Références")]
    [SerializeField] private RectTransform destinationPoint;

    [Header("Paramètres du Drag & Drop")]
    [SerializeField] private float snapDistance = 100f;
    [SerializeField] private float returnDuration = 0.5f;
    [SerializeField] private float snapDuration = 0.3f;

    [Header("Paramètres d'animation")]
    [SerializeField] private float hintAnimDuration = 0.5f;
    [SerializeField] private float hintAnimDistance = 50f;
    [SerializeField] private float hintAnimInterval = 2f;
    [SerializeField] private Ease hintEaseDown = Ease.InOutQuad;
    [SerializeField] private Ease hintEaseUp = Ease.InOutQuad;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 initialAnchoredPosition;
    private Sequence hintSequence;
    private Vector2 dragOffset;
    private Camera canvasCamera;
    bool anchored = false;

    private RectTransform ParentRectTransform => rectTransform.parent as RectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
        {
            Debug.LogError("Ce script doit être attaché à un élément UI dans un Canvas!");
            enabled = false;
            return;
        }

        canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    private void Start()
    {
        initialAnchoredPosition = rectTransform.anchoredPosition;
        StartHintAnimation();
    }

    private void OnDisable()
    {
        if(!anchored)
            ReturnToInitialPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        anchored = false ;
        StopHintAnimation();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTransform, eventData.position, canvasCamera, out Vector2 localPointerPos))
        {
            dragOffset = (Vector2)rectTransform.localPosition - localPointerPos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTransform, eventData.position, canvasCamera, out Vector2 localPointerPos))
        {
            rectTransform.localPosition = localPointerPos + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        Vector2 objScreenPos = RectTransformUtility.WorldToScreenPoint(canvasCamera, rectTransform.position);
        Vector2 destScreenPos = RectTransformUtility.WorldToScreenPoint(canvasCamera, destinationPoint.position);

        if (Vector2.Distance(objScreenPos, destScreenPos) <= snapDistance)
            SnapToDestination();
        else
            ReturnToInitialPosition();
    }

    private void SnapToDestination()
    {
        anchored = true;
        if (destinationPoint.parent != rectTransform.parent)
        {
            Vector3 worldDestPos = destinationPoint.TransformPoint(Vector3.zero);
            Vector3 localDestPos = ParentRectTransform.InverseTransformPoint(worldDestPos);
            rectTransform.DOLocalMove(localDestPos, snapDuration).SetEase(Ease.OutBack);
        }
        else
        {
            rectTransform.DOAnchorPos(destinationPoint.anchoredPosition, snapDuration).SetEase(Ease.OutBack);
        }
        MatchTheStarsController.Instance.StartVerification();
        this.enabled = false;
    }

    private void ReturnToInitialPosition()
    {
        rectTransform.DOAnchorPos(initialAnchoredPosition, returnDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(StartHintAnimation);
    }

    private void StartHintAnimation()
    {
        StopHintAnimation();

        rectTransform.anchoredPosition = initialAnchoredPosition;

        hintSequence = DOTween.Sequence()
            .Append(rectTransform.DOAnchorPosY(initialAnchoredPosition.y - hintAnimDistance, hintAnimDuration).SetEase(hintEaseDown))
            .Append(rectTransform.DOAnchorPosY(initialAnchoredPosition.y, hintAnimDuration).SetEase(hintEaseUp))
            .AppendInterval(hintAnimInterval)
            .SetLoops(-1);
    }

    private void StopHintAnimation()
    {
        if (hintSequence != null)
        {
            hintSequence.Kill();
            hintSequence = null;
        }
    }

    private void OnDestroy()
    {
        StopHintAnimation();
    }
}
