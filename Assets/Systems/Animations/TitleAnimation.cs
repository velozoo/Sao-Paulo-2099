using DG.Tweening;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform rt;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private Ease upDownEase;

    private Sequence upDownSequence;

    private Vector2 startPos;
    private void Start()
    {
        startPos = rt.anchoredPosition;
        UpDownAnim();
    }

    public void UpDownAnim()
    {
        upDownSequence?.Kill();

        rt.anchoredPosition = new Vector2(
            startPos.x,
            startPos.y + maxY
        );

        upDownSequence = DOTween.Sequence();

        upDownSequence.Append(
            rt.DOAnchorPosY(startPos.y + minY, upDownSpeed)
            .SetEase(upDownEase)
        );

        upDownSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
