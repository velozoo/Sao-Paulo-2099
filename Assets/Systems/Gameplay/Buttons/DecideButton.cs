using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class DecideButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private bool isPass;

    [Header("Animation")]
    [SerializeField] private RectTransform button;
    [SerializeField] private float pressDistance = 5f;
    [SerializeField] private float pressDuration = 0.08f;
    [SerializeField] private float releaseDuration = 0.2f;
    [SerializeField] private Ease pressEase = Ease.OutQuad;
    [SerializeField] private Ease releaseEase = Ease.OutBack;

    private float originalY;
    private bool isHolding;
    private bool isHovering;
    private bool canDecide;

    private void Awake()
    {
        originalY = button.anchoredPosition.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        canDecide = false;
        AnimateButton(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        if (isHolding)
            AnimateButton(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (isHolding)
            AnimateButton(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHolding)
            return;

        isHolding = false;

        if (isHovering && canDecide)
        {
            if (isPass)
                PassengerValidator.Instance.Pass();
            else
                PassengerValidator.Instance.NotPass();
        }

        AnimateButton(false);
    }

    private void AnimateButton(bool pressed)
    {
        button.DOKill();

        float targetY = originalY;

        if (pressed)
            targetY -= pressDistance;

        float duration = pressed ? pressDuration : releaseDuration;
        Ease ease = pressed ? pressEase : releaseEase;

        button.DOAnchorPosY(targetY, duration)
            .SetEase(ease)
            .OnComplete(() =>
            {
                if (pressed && isHolding && isHovering)
                    canDecide = true;
            });
    }
}