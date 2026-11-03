using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    private TMP_Text _textBox;

    // Just testing
    [Header("Test String")]
    [SerializeField] private string testText;

    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Setting")]
    [SerializeField] private float characterPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;

    
    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        if (characterPerSecond <= 0)
            characterPerSecond = 20;

        _simpleDelay = new WaitForSeconds(1 / characterPerSecond);

        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
    }

    private void Start()
    {
        SetText(testText);
    }

    public void SetText(string text)
    {
        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.text = text;
        _textBox.ForceMeshUpdate();
        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;

            if (character == '?' ||  character == '.' || character == ',' || character == ';' ||
                character == ':' || character == '!' || character == '-')
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return _simpleDelay;
            }

            _currentVisibleCharacterIndex++;
        }

        _typewriterCoroutine = null;
    }
}
