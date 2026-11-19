using DG.Tweening;
using System;
using System.Collections;
using TMPEffects.CharacterData;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
public class DialogueManager : MonoBehaviour
{
    [Header("Passenger Text")]
    [SerializeField] private TMP_Text passengerText;
    [SerializeField] private TMPWriter passengerWriter;
    [SerializeField] private string passengerEffect;
    [SerializeField] private GameObject passTextGO;

    [Header("Inspector Text")]
    [SerializeField] private TMP_Text inspectorText;
    [SerializeField] private TMPWriter inspectorWriter;
    [SerializeField] private string inspectorEffect;
    [SerializeField] private GameObject inspTextGO;

    [Header("Inspector Lines")]
    [SerializeField] private string[] entryLines;
    [SerializeField] private string[] avoidLines;
    [SerializeField] private string[] passLines;

    [Header("Delay")]
    [SerializeField] private float interpunctuationDelay;
    [SerializeField] private float finalSetenceDelay;
    [SerializeField] private float finalTextDelay;

    [Header("Audio")]
    [SerializeField] private AudioClip talkClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    private Coroutine _nextTextCoroutine;
    private bool _isPaused;
    private Action _onDialogueComplete;
    private GenericPassengerData passenger;
    private Sequence toggleSequence;

    private bool passengerTalking;
    private bool inspectorTalking;

    public void BeginDialogue(Action onComplete)
    {
        passenger = PassengerManager.Instance._currentPassenger;

        _onDialogueComplete = onComplete;

        ResetAll();
        StartCoroutine(EntryDialogue());
    }

    private IEnumerator EntryDialogue()
    {
        ShowText();
        passengerText.text = passenger.entryLine;
        AdjustText(passengerText);
        passengerText.text = passengerEffect + passenger.entryLine;

        passengerWriter.StartWriter();

        yield return new WaitUntil(() => !passengerTalking);

        yield return new WaitForSeconds(finalTextDelay);

        ResetAll();

        HideText();
        //ShowText(inspTextGO);
        inspTextGO.SetActive(true);

        int i = UnityEngine.Random.Range(0, entryLines.Length);
        inspectorText.text = inspectorEffect + entryLines[i];
        inspectorWriter.StartWriter();

        yield return new WaitUntil(() => !inspectorTalking);

        yield return new WaitForSeconds(finalTextDelay);

        inspTextGO.SetActive(false);

        ResetAll();
        _onDialogueComplete?.Invoke();
        _onDialogueComplete = null;
    }

    public void AdjustText(TMP_Text text)
    {
        text.ForceMeshUpdate();

        float preferredHeight = text.preferredHeight;

        text.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            preferredHeight
        );
    }

    public void FinishDialogue(bool isAvoided, Action onComplete)
    {
        _onDialogueComplete = onComplete;

        ResetAll();
        StartCoroutine(ExitDialogue(isAvoided));
    }

    private IEnumerator ExitDialogue(bool isAvoided)
    {
        if (isAvoided)
        {
            ShowText();
            passengerText.text = passenger.avoidLine;
            AdjustText(passengerText);
            passengerText.text = passengerEffect + passenger.avoidLine;

            passengerWriter.StartWriter();


            yield return new WaitUntil(() => !passengerTalking);

            yield return new WaitForSeconds(finalTextDelay);

            ResetAll();

            HideText();
            inspTextGO.SetActive(true);

            int i = UnityEngine.Random.Range(0, avoidLines.Length);
            inspectorText.text = inspectorEffect + avoidLines[i];
            inspectorWriter.StartWriter();

            yield return new WaitUntil(() => !inspectorTalking);

            yield return new WaitForSeconds(finalTextDelay);

            inspTextGO.SetActive(false);

            ResetAll();
            _onDialogueComplete?.Invoke();
            _onDialogueComplete = null;
        }
        else
        {
            ShowText();
            passengerText.text = passenger.avoidLine;
            AdjustText(passengerText);
            passengerText.text = passengerEffect + passenger.avoidLine;

            passengerWriter.StartWriter();

            yield return new WaitUntil(() => !passengerTalking);

            yield return new WaitForSeconds(finalTextDelay);
            HideText();

            ResetAll();
            _onDialogueComplete?.Invoke();
            _onDialogueComplete = null;
        }
    }

    public void PauseAtEndPhrase()
    {
        if (_nextTextCoroutine != null)
            return;

        _nextTextCoroutine = StartCoroutine(NextTextDelay());
    }

    private IEnumerator NextTextDelay()
    {
        yield return new WaitForSeconds(finalTextDelay);

        _nextTextCoroutine = null;
    }

    public void OnCharacterShown(TMPWriter writer, CharData character)
    {
        char currentChar = character.info.character;

        if (currentChar == ',' || currentChar == ';' || currentChar == ':')
        {
            PauseText(writer, interpunctuationDelay);
        }
        else if (currentChar == '?' || currentChar == '.' || currentChar == '!')
        {
            PauseText(writer, finalSetenceDelay);
        }
    }
    private void PauseText(TMPWriter writer, float time)
    {
        if (_isPaused)
            return;

        StartCoroutine(PauseCoroutine(writer, time));
    }

    private IEnumerator PauseCoroutine(TMPWriter writer, float time)
    {
        _isPaused = true;

        writer.StopWriter();

        yield return new WaitForSeconds(time);

        writer.StartWriter();

        _isPaused = false;
    }

    public void ResetAll()
    {
        passengerWriter.ResetWriter();
        inspectorWriter.ResetWriter();
    }

    public void ShowText()
    {
        toggleSequence?.Kill();

        toggleSequence = DOTween.Sequence();

        passTextGO.transform.localScale = Vector2.zero;
        passTextGO.SetActive(true);

        toggleSequence.Append(passTextGO.transform.DOScale(Vector2.one, .5f));
    }
    public void HideText()
    {
        toggleSequence?.Kill();

        toggleSequence = DOTween.Sequence();

        toggleSequence.Append(passTextGO.transform.DOScale(Vector2.zero, .5f));

        toggleSequence.OnComplete(() =>
        {
            passTextGO.SetActive(false);
        });
    }

    public void PassengerTalking(bool a)
    {
        passengerTalking = a;
    }

    public void InspectorTalking(bool a)
    {
        inspectorTalking = a;
        print(inspectorTalking);
    }

    public void PlayInspTalkClip()
    {
        SoundFXManager.Instance.PlaySoundFXClip(talkClip, volume, transform);
    }
    public void PlayPassTalkClip()
    {
        if (passenger.voice.Length == 1)
            SoundFXManager.Instance.PlaySoundFXClip(passenger.voice[0], volume, transform);
        else if (passenger.voice.Length > 1)
            SoundFXManager.Instance.PlayRandomSoundFXClip(passenger.voice, volume, transform);
        else
            return;
    }
}
