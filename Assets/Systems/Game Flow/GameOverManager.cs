using NUnit.Framework.Constraints;
using System.Collections;
using TMPEffects.Components;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private AudioClip music;
    [SerializeField] private float musicVolume;
    [SerializeField] private float musicDelay;
    [SerializeField] private GameObject title;
    [SerializeField] private float titleDelay;
    [SerializeField] private TMPWriter subtitleWriter;
    [SerializeField] private float subtitleDelay;
    [SerializeField] private GameObject buttons;
    [SerializeField] private float buttonsDelay;

    public void DoGameOver()
    {
        title.SetActive(false);
        subtitleWriter.ResetWriter();
        buttons.SetActive(false);
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        GameManager.Instance.ResetGuesses();
        panel.SetActive(true);
        yield return new WaitForSeconds(musicDelay);
        SoundFXManager.Instance.PlaySoundFXReverbClip(music, musicVolume);
        yield return new WaitForSeconds(titleDelay);    
        title.SetActive(true);
        yield return new WaitForSeconds(subtitleDelay);
        subtitleWriter.StartWriter();
        yield return new WaitForSeconds(buttonsDelay);
        buttons.SetActive(true);
    }

    public void ResetAll()
    {
        panel.SetActive(false);
        title.SetActive(false);
        subtitleWriter.ResetWriter();
        buttons.SetActive(false);
    }
}
