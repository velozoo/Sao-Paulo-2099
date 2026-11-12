using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DesktopScreen : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject desktopScreen;
    [SerializeField] private float openScreenTime;

    private void Start()
    {
        desktopScreen.SetActive(false);
        desktopScreen.transform.localScale = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenScreen();
    }

    public void OpenScreen()
    {
        desktopScreen.SetActive(true);
        desktopScreen.transform.DOScale(new Vector3(1, 1, 1), openScreenTime).SetEase(Ease.InOutSine);
    }

    public void CloseScreen()
    {
        desktopScreen.transform.DOScale(Vector3.zero, openScreenTime).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            desktopScreen.SetActive(false);
        });
    }
}
