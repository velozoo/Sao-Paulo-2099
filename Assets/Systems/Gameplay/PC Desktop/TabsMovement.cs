using UnityEngine;
using UnityEngine.EventSystems;

public class TabsMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject tab;
    [SerializeField] private GameObject UIClickPosPrefab;
    [SerializeField] private Camera canvasCamera;

    private GameObject OriginalParent, UIClick;

    private float _distanceFromCamera;

    private void Awake()
    {
        OriginalParent = tab.transform.parent.gameObject;
        _distanceFromCamera = Mathf.Abs(canvasCamera.transform.position.z - tab.transform.position.z);
    }
    private void Start()
    {
        if (canvasCamera == null)
            canvasCamera = Camera.main;
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 screenPoint = new Vector3(
            screenPosition.x,
            screenPosition.y,
            _distanceFromCamera
        );

        return canvasCamera.ScreenToWorldPoint(screenPoint);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UIClick == null)
            UIClick = Instantiate(UIClickPosPrefab, tab.transform.parent);

        UIClick.transform.position = GetWorldPosition(eventData.position);

        tab.transform.SetParent(UIClick.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIClick.transform.position = GetWorldPosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tab.transform.SetParent(OriginalParent.transform);
    }
}
