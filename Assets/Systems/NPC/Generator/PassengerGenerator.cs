using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public enum TypesOfProblems
{
    None,
    Appearance,
    WrongDestination,
    DigitalFilliated,
    InconsistentCredential,
    InaccessibleDestination,
    LarpGram,
    NoEasyPass,
    MalfunctionEasyPass,
}

public class PassengerGenerator : MonoBehaviour
{
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private Image passengerImage;
    [SerializeField] private RectTransform passengerRectTrans;

    [Header("Walk Anim")]
    [SerializeField] private float inPos = -512;
    [SerializeField] private float stopPos = 0;
    [SerializeField] private float outPos = 512;
    [SerializeField] private float walkDuration;
    [SerializeField] private int jumps;
    [SerializeField] private float walkMaxY;

    [Header("Idle Anim")]
    [SerializeField] private float idleMinY;
    [SerializeField] private float idleMaxY;
    [SerializeField] private float idleSpeed;
    [SerializeField] private Ease idleEase;


    private float initialPosY;
    DG.Tweening.Sequence _idleSequence;

    private void Start()
    {
        initialPosY = idleMinY;

        passengerRectTrans.anchoredPosition = new Vector2(inPos, initialPosY);
    }

    public void InstantiatePassenger(GenericPassengerData passenger)
    {
        if (passengerImage == null)
            return;

        // -512 a 0 dps 0 a 512
        passengerImage.sprite = passenger.sprite;

        // passengerImage.preserveAspect = true;
        if (passenger.larpGramData != null)
        {
            LarpGramManager.Instance.DestroyPost();
            LarpGramManager.Instance.UpdateLarp(passenger);
        }
        DocumentsManager.Instance.UpdateDocuments(passenger);
    }

    public void StartIdle()
    {
        _idleSequence?.Kill();
        passengerRectTrans.DOKill();

        _idleSequence = DOTween.Sequence();

        _idleSequence.Append(passengerRectTrans.DOAnchorPosY(idleMaxY, idleSpeed).SetEase(idleEase));
        _idleSequence.Append(passengerRectTrans.DOAnchorPosY(idleMinY, idleSpeed).SetEase(idleEase));

        _idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        _idleSequence?.Kill();
    }

    public void MoveInPassenger(Action onComplete)
    {
        StopIdle();
        passengerRectTrans.DOKill();

        passengerRectTrans.anchoredPosition = new Vector2(inPos, initialPosY);

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(passengerRectTrans.DOAnchorPosX(stopPos, walkDuration).SetEase(Ease.InOutSine));
        sequence.Join(passengerRectTrans.DOAnchorPosY(initialPosY + walkMaxY, walkDuration / jumps)
            .SetLoops(jumps, LoopType.Yoyo).SetEase(Ease.InOutSine));

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            StartIdle();
        });
    }
    public void MoveOutPassenger(Action onComplete)
    {
        StopIdle();
        passengerRectTrans.DOKill();

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(passengerRectTrans.DOAnchorPosX(outPos, walkDuration).SetEase(Ease.InCubic));
        sequence.Join(passengerRectTrans.DOAnchorPosY(initialPosY + walkMaxY, walkDuration / jumps)
            .SetLoops(jumps, LoopType.Yoyo).SetEase(Ease.InOutSine));

        sequence.OnComplete(() => onComplete?.Invoke());
    }

    public void MoveAvoidPassenger(Action onComplete)
    {
        StopIdle();
        passengerRectTrans.DOKill();

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(passengerRectTrans.DOAnchorPosX(inPos, walkDuration).SetEase(Ease.InCubic));
        sequence.Join(passengerRectTrans.DOAnchorPosY(initialPosY + walkMaxY, walkDuration / jumps)
            .SetLoops(jumps, LoopType.Yoyo).SetEase(Ease.InOutSine));

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
