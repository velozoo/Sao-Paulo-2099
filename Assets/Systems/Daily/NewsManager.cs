using System;
using System.Collections;
using TMPEffects.CharacterData;
using TMPEffects.Components;
using TMPro;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private NewsAnimation newsAnimation;

    [Header("Bom Dia Text")]
    [SerializeField] private TMP_Text bdsText;
    [SerializeField] private TMPWriter bdsWriter;
    [SerializeField] private string catchPhrase;

    [Header("Dialogue Text")]
    [SerializeField] private NewsData[] newsData;
    [SerializeField] private TMP_Text newsText;
    [SerializeField] private TMPWriter newsWriter;
    [SerializeField] private string newsEffectStart;

    [Header("End Text")]
    [SerializeField] private TMP_Text endText;
    [SerializeField] private TMPWriter endWriter;
    [SerializeField] private string[] endPhrase;

    [Header("Delay")]
    [SerializeField] private float interpunctuationDelay;
    [SerializeField] private float finalSetenceDelay;
    [SerializeField] private float finalTextDelay;

    [Header("Audio")]
    [SerializeField] private AudioClip talkClip;
    [SerializeField] private AudioClip[] randomTalkClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    private int _currentTextLineShow;
    private int _currentDay;
    private Coroutine _nextTextCoroutine;
    private bool _isPaused;

    private Action _onNewsComplete;
    void Start()
    {
        ResetAll();
    }

    public void BeginNews(Action onComplete)
    {
        newsAnimation.OpenScreen(() =>
        {
            _onNewsComplete = onComplete;

            _currentDay = GameManager.Instance._currentDay;
            _currentDay -= 1;
            print(newsData[_currentDay]);
            ResetAll();
            PlayCatchPhrase();
        });
    }

    public void PlayCatchPhrase()
    {
        bdsText.text = catchPhrase;
        bdsWriter.StartWriter();
    }

    public void PlayEndPhrase()
    {
        if(_currentTextLineShow >= endPhrase.Length)
        {
            _currentTextLineShow = 0;
            ResetAll();

            newsAnimation.CloseScreen(() =>
            {
                _onNewsComplete?.Invoke();
                _onNewsComplete = null;
            });
            return;
        }

        endText.text = endPhrase[_currentTextLineShow];
        endWriter.StartWriter();
        _currentTextLineShow++;
    }

    public void NextText()
    {
        if (_currentTextLineShow >= newsData[_currentDay].dialogueLines.Length)
        {
            _currentTextLineShow = 0;
            ResetAll();
            PlayEndPhrase();
            return;
        }
        newsText.text = newsEffectStart + newsData[_currentDay].dialogueLines[_currentTextLineShow];
        newsWriter.StartWriter();
        _currentTextLineShow++;
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

        NextText();

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
        bdsWriter.ResetWriter();
        newsWriter.ResetWriter();
        endWriter.ResetWriter();
    }

    public void PlayTalkClip()
    {
        SoundFXManager.Instance.PlaySoundFXClip(talkClip, volume, transform);
    }
    public void PlayRandomTalkClip()
    {
        SoundFXManager.Instance.PlayRandomSoundFXClip(randomTalkClip, volume, transform);
    }
}
