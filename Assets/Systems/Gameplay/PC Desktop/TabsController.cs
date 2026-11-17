
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        foreach (var tab in tabsRT)
        {
            _tabsPos.Add(tab.anchoredPosition);
        }
    }

    public void OpenTab(GameObject tab, RectTransform initialPos, Vector2 finalPos, Action onComplete)
    {
        RectTransform rtTab = tab.GetComponent<RectTransform>();

        Vector3 worldPos = initialPos.TransformPoint(initialPos.rect.center);
        Vector3 localPos = rtTab.parent.InverseTransformPoint(worldPos);

        rtTab.anchoredPosition = new Vector2(localPos.x, localPos.y);

        tab.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(tab.transform.DOScale(Vector3.one, openTabTime).SetEase(Ease.InOutSine));
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

        Vector3 worldPos = finalPos.TransformPoint(finalPos.rect.center);
        Vector3 localPos = rtTab.parent.InverseTransformPoint(worldPos);

        Vector2 targetPos = new Vector2(localPos.x, localPos.y);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(tab.transform.DOScale(Vector3.zero, openTabTime).SetEase(Ease.InOutSine));
        sequence.Join(rtTab.DOAnchorPos(targetPos, openTabTime).SetEase(Ease.InOutSine));

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
