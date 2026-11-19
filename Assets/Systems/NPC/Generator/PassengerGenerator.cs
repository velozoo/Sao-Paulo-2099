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
    [SerializeField] private Image passengerImage;
    [SerializeField] private RectTransform passengerRectTrans;
    [SerializeField] private DialogueManager dialogueManager;

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
    private float _currentPosY;
    DG.Tweening.Sequence _idleSequence;

    private void Start()
    {
        initialPosY = idleMinY;

        passengerRectTrans.anchoredPosition = new Vector2(inPos, initialPosY);
    }

    public void InstantiatePassenger(GenericPassengerData passenger)
    {
        if (passenger == null)
            return;

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

        ScanManager.Instance.UpdatePassenger(passenger);

        DocumentsManager.Instance.UpdateDocuments(passenger);
    }

    public void StartIdle()
    {
        _idleSequence?.Kill();
        passengerRectTrans.DOKill();

        _idleSequence = DOTween.Sequence();

        _idleSequence.Append(passengerRectTrans.DOAnchorPosY(idleMaxY, idleSpeed).SetEase(idleEase));
        _idleSequence.Append(passengerRectTrans.DOAnchorPosY(idleMinY, idleSpeed).SetEase(idleEase));

        _idleSequence.SetLoops(-1, LoopType.Yoyo);
    }

    public void StopIdle()
    {
        _idleSequence?.Kill();
    }

    public void IdleTransition(Action onComplete)
    {
        _currentPosY = passengerRectTrans.anchoredPosition.y;

        if (_currentPosY == initialPosY) 
            onComplete?.Invoke();

        float distance = MathF.Abs(_currentPosY - initialPosY);
        float totalDistance = MathF.Abs(idleMaxY - initialPosY);

        float duration = idleSpeed * (distance / totalDistance);

        passengerRectTrans.DOAnchorPosY(initialPosY, duration / 2).SetEase(idleEase).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
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
            IdleTransition(StartIdle);
            dialogueManager.BeginDialogue(() => onComplete?.Invoke());
        });
    }

    public void MoveOutPassenger(Action onComplete)
    {
        dialogueManager.FinishDialogue(false, () =>
        {
            IdleTransition(() =>
            {
                StopIdle();
                passengerRectTrans.DOKill();

                DG.Tweening.Sequence sequence = DOTween.Sequence();

                sequence.Append(passengerRectTrans.DOAnchorPosX(outPos, walkDuration).SetEase(Ease.InCubic));
                sequence.Join(passengerRectTrans.DOAnchorPosY(initialPosY + walkMaxY, walkDuration / jumps)
                    .SetLoops(jumps, LoopType.Yoyo).SetEase(Ease.InOutSine));

                sequence.OnComplete(() => onComplete?.Invoke());
            });
        });
        
    }

    public void MoveAvoidPassenger(Action onComplete)
    {
        dialogueManager.FinishDialogue(true, () =>
        {
            IdleTransition(() =>
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
            });
        });
    }
}
