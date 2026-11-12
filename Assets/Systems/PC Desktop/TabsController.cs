using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TabsController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject buttonDocuments;
    [SerializeField] private GameObject buttonLarpGram;
    [SerializeField] private GameObject buttonEasyPass;


    [Header("Animation Config")]
    [SerializeField] private float openTabTime;

    [Header("Sound")]
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private AudioClip openClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    public static TabsController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenTab(GameObject tab, RectTransform initialPos, Vector2 finalPos, Action onComplete)
    {
        RectTransform rtTab = tab.GetComponent<RectTransform>();

        rtTab.anchoredPosition = initialPos.anchoredPosition;

        tab.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(tab.transform.DOScale(new Vector3(1, 1, 1), openTabTime).SetEase(Ease.InOutSine));
        sequence.Join(rtTab.DOAnchorPos(finalPos, openTabTime).SetEase(Ease.InOutSine));
        SoundFXManager.Instance.PlaySoundFXClip(openClip, volume);

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void CloseTab(GameObject tab, RectTransform finalPos, Action onComplete)
    {
        RectTransform rtTab = tab.GetComponent<RectTransform>();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(tab.transform.DOScale(new Vector3(0, 0, 0), openTabTime).SetEase(Ease.InOutSine));
        sequence.Join(rtTab.DOAnchorPos(finalPos.anchoredPosition, openTabTime).SetEase(Ease.InOutSine));
        SoundFXManager.Instance.PlaySoundFXClip(closeClip, volume);

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            tab.SetActive(false);
        });
    }
}
