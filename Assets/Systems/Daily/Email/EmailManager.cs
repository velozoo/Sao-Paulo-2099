using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    [Header("Email Data")]
    [SerializeField] private List<EmailUnlock> emailUnlocks;

    [Header("Text")]
    [SerializeField] private TMP_Text fromText;
    [SerializeField] private TMP_Text toText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text contentText;

    [Header("Notify Anim")]
    [SerializeField] private GameObject exclamationGO;
    [SerializeField] private GameObject exclamationGOpc;
    [SerializeField] private RectTransform exclamationRT;
    [SerializeField] private float startPosY;
    [SerializeField] private float finalPosY;
    [SerializeField] private float notifyTime;
    [SerializeField] private Ease easeNotify;
    private Sequence notifySequence;

    [Header("Sound")]
    [SerializeField] private AudioClip newEmailSound;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] private float soundDelay = 0.5f;

    private List<SuperiorInstructionData> _currentEmailData = new List<SuperiorInstructionData>();
    private SuperiorInstructionData lastEmail = null;

    private int _currentDay;
    public void StartEmail()
    {
        _currentDay = GameManager.Instance._currentDay;

        CheckNewEmails();
    }

    public void CheckNewEmails()
    {
        int newEmails = 0;

        foreach (EmailUnlock email in emailUnlocks)
        {
            bool available = IsEmailAvailable(email);

            email.emailGO.SetActive(available);

            if (available && !_currentEmailData.Contains(email.emailData))
            {
                _currentEmailData.Add(email.emailData);
                lastEmail = email.emailData;

                newEmails++;
            }
        }

        if (newEmails > 0)
        {
            SetEmail(lastEmail);
            StartCoroutine(PlayNewEmailSounds(newEmails));
        }
    }

    public void SetEmail(SuperiorInstructionData emailData)
    {
        fromText.text = emailData.emailFrom;
        toText.text = emailData.emailFor;
        subjectText.text = emailData.emailSubject;
        contentText.text = emailData.emailContent;

        contentText.rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical, contentText.rectTransform.rect.width);
    }

    private bool IsEmailAvailable(EmailUnlock emailData)
    {
        return _currentDay >= emailData.unlockDay;
    }

    private IEnumerator PlayNewEmailSounds(int amount)
    {
        ShowExclamation();
        for (int i = 0; i < amount; i++)
        {
            SoundFXManager.Instance.PlaySoundFXClip(newEmailSound, volume);

            yield return new WaitForSeconds(soundDelay);
        }
    }

    public void ShowExclamation()
    {
        notifySequence?.Kill();

        exclamationRT.anchoredPosition = new Vector2(exclamationRT.anchoredPosition.x, startPosY);
        exclamationGO.transform.localScale = Vector2.zero;
        exclamationGOpc.transform.localScale = Vector2.zero;

        exclamationGOpc.SetActive(true);
        exclamationGO.SetActive(true);

        notifySequence = DOTween.Sequence();

        notifySequence.Append(exclamationRT.DOAnchorPosY(finalPosY, notifyTime).SetEase(easeNotify));
        notifySequence.Join(exclamationGO.transform.DOScale(Vector2.one * 3.44f, notifyTime).SetEase(easeNotify));
        notifySequence.Join(exclamationGOpc.transform.DOScale(Vector2.one, notifyTime).SetEase(easeNotify));
    }

    public void HideExclamation()
    {
        notifySequence?.Kill();

        notifySequence = DOTween.Sequence();

        notifySequence.Append(exclamationRT.DOAnchorPosY(startPosY, notifyTime).SetEase(easeNotify));
        notifySequence.Join(exclamationGO.transform.DOScale(Vector2.zero, notifyTime * .5f).SetEase(easeNotify));
        notifySequence.Join(exclamationGOpc.transform.DOScale(Vector2.zero, notifyTime * .5f).SetEase(easeNotify));

        notifySequence.OnComplete(() =>
        {
            exclamationGOpc.SetActive(false);
            exclamationGO.SetActive(false);
        });
    }
}
