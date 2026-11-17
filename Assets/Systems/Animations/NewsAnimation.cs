using UnityEngine;
using DG.Tweening;
using System;

public class NewsAnimation : MonoBehaviour
{
    [SerializeField] private GameObject screen;

    [Header("Toggle Animation")]
    [SerializeField] private float toggleSpeed;
    [SerializeField] private Ease toggleEase;
    [SerializeField] private float waitToBegin;

    [Header("Idle Animation")]
    [SerializeField] private float idleSpeed;
    [SerializeField] private Ease idleEase;
    [SerializeField] private float idleMaxY, idleMinY;

    private RectTransform rt;
    private float _startPos;
    private Sequence _idleSequence;
    private Sequence _toggleSequence;


    private void Start()
    {
        
        rt = screen.GetComponent<RectTransform>();
        _startPos = rt.anchoredPosition.y;

        screen.SetActive(false);
    }

    public void OpenScreen(Action onComplete)
    {
        _toggleSequence?.Kill();
        screen.transform.DOKill();

        screen.transform.localScale = Vector3.zero;
        screen.SetActive(true);

        _toggleSequence = DOTween.Sequence();

        _toggleSequence.Append(screen.transform.DOScale(new Vector2(1.2f, 1.2f), .2f).SetEase(toggleEase));
        _toggleSequence.Append(screen.transform.DOScale(Vector2.one, toggleSpeed).SetEase(toggleEase).OnComplete(() =>
        {
            StartIdle();
            DOVirtual.DelayedCall(waitToBegin, () =>
            {
                onComplete?.Invoke();
            });
        }));
    }

    public void StopIdle()
    {
        _idleSequence?.Kill();
    }

    public void StartIdle()
    {
        StopIdle();

        _idleSequence = DOTween.Sequence();

        _idleSequence.Append(rt.DOAnchorPosY(idleMaxY, idleSpeed).SetEase(idleEase));
        _idleSequence.Append(rt.DOAnchorPosY(idleMinY, idleSpeed).SetEase(idleEase));

        _idleSequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void IdleTransition(Action onComplete)
    {
        StopIdle();

        float currentPosY = rt.anchoredPosition.y;

        if(currentPosY == _startPos)
            onComplete?.Invoke();

        float distance = MathF.Abs(currentPosY - _startPos);
        float totalDistance = MathF.Abs(idleMaxY - _startPos);

        float duration = idleSpeed * (distance / totalDistance);

        rt.DOAnchorPosY(_startPos, duration / 5).SetEase(idleEase).OnComplete(() => {
            onComplete?.Invoke();
        });

    }

    public void CloseScreen(Action onComplete)
    {
        _toggleSequence?.Kill();
        screen.transform.DOKill();

        IdleTransition(() =>
        {
            _toggleSequence.Append(screen.transform.DOScale(new Vector2(1.2f, 1.2f), .2f).SetEase(toggleEase));
            _toggleSequence.Append(screen.transform.DOScale(Vector2.zero, toggleSpeed).SetEase(toggleEase).OnComplete(() =>
            {
                screen.SetActive(false);
                onComplete?.Invoke();
            }));
        });
    }
}
