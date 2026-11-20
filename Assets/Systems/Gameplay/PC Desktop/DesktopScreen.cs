using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class DesktopScreen : MonoBehaviour, IPointerClickHandler
{
    [Header("Screen")]
    [SerializeField] private GameObject desktopScreen;
    [SerializeField] private float openScreenTime;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private CanvasGroup cg;

    [Header("Zoom Turn On")]
    [SerializeField] private GameObject gameContent;
    [SerializeField] private float zoomTime;
    [SerializeField] private float zoomScale;
    [SerializeField] private float zoomPosY;
    [SerializeField] private Ease scaleEase;
    [SerializeField] private Ease posEase;
    [SerializeField] private float waitToOpen;
    [SerializeField] private float fadeDuration;
    [SerializeField] private AudioClip turnOnClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    public bool _hasOpenToday = false;

    private void Start()
    {
        ResetPC();
    }

    public void ResetPC()
    {
        cg.alpha = 0;
        blackScreen.SetActive(false);
        _hasOpenToday = false;
        desktopScreen.transform.localScale = Vector3.zero;
        desktopScreen.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hasOpenToday)
            OpenScreen();
        else
            TurnOn();
    }
    public void TurnOn()
    {
        Sequence _turnOnSequence = DOTween.Sequence();

        Vector3 scale = Vector3.one * zoomScale;
        RectTransform rt = gameContent.GetComponent<RectTransform>();
        Vector3 initialScale = gameContent.transform.localScale;
        Vector2 initialPos = rt.anchoredPosition;

        _hasOpenToday = true;

        blackScreen.SetActive(true);
        _turnOnSequence.Append(gameContent.transform.DOScale(scale, zoomTime).SetEase(scaleEase));
        _turnOnSequence.Join(DOTween.To(() => cg.alpha, x => cg.alpha = x, 1, fadeDuration));
        _turnOnSequence.Join(rt.DOAnchorPosY(zoomPosY, zoomTime).SetEase(posEase));

        _turnOnSequence.OnComplete(() =>
        {
            gameContent.transform.localScale = initialScale;
            rt.anchoredPosition = initialPos;
            SoundFXManager.Instance.PlaySoundFXClip(turnOnClip, volume);
            DOVirtual.DelayedCall(waitToOpen, () =>
            {

                OpenScreen();
            });

        });
    }

    public void OpenScreen()
    {
        desktopScreen.SetActive(true);
        desktopScreen.transform.DOScale(new Vector3(1, 1, 1), openScreenTime).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                blackScreen.SetActive(false);
                DOTween.To(() => cg.alpha, x => cg.alpha = x, 0, 0);
            });
    }

    public void CloseScreen()
    {
        desktopScreen.transform.DOScale(Vector3.zero, openScreenTime).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            desktopScreen.SetActive(false);
        });
    }

}
