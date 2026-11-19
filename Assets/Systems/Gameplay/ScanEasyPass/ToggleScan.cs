using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleScan : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool close;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PassengerManager.Instance.CanMakeDecision == false)
            return;

        if (close)
        {
            print("fecha");
            ScanManager.Instance.HideScan();
        }
        else
        {
            print("abre");
            ScanManager.Instance.ShowScan();
        }
    }
}
