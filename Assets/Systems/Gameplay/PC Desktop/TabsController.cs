using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    [Header("Documents Tabs")]
    [SerializeField] private List<RectTransform> tabsRT;
    public List<Vector2> _tabsPos = new List<Vector2>();

    public static TabsController Instance;

    private void Awake()
    {
        Instance = this;

        foreach(var tab in tabsRT)
        {
            _tabsPos.Add(tab.anchoredPosition);
        }
    }

    public void OpenTab(GameObject tab, RectTransform initialPos, Vector2 finalPos, Action onComplete)
    {
        RectTransform rtTab = tab.GetComponent<RectTransform>();

        rtTab.anchoredPosition = initialPos.anchoredPosition;

        tab.SetActive(true);

        DG.Tweening.Sequence sequence = DOTween.Sequence();

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

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(tab.transform.DOScale(new Vector3(0, 0, 0), openTabTime).SetEase(Ease.InOutSine));
        sequence.Join(rtTab.DOAnchorPos(finalPos.anchoredPosition, openTabTime).SetEase(Ease.InOutSine));
        SoundFXManager.Instance.PlaySoundFXClip(closeClip, volume);

        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
            ResetDocumentsTabs();
            tab.SetActive(false);
        });
    }

    private void ResetDocumentsTabs()
    {
        for (int i = 0; i < _tabsPos.Count; i++)
        {
            tabsRT[i].anchoredPosition = _tabsPos[i];
        } 
    }
}
