using UnityEngine;
using UnityEngine.EventSystems;

public class TabsMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private GameObject tab;
    [SerializeField] private GameObject UIClickPosPrefab;

    private GameObject OriginalParent, UIClick;

    private void Awake()
    {
        OriginalParent = tab.transform.parent.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(UIClick == null)
        UIClick = Instantiate(UIClickPosPrefab, tab.transform.parent);

        UIClick.transform.position = eventData.position;
        tab.transform.SetParent(UIClick.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIClick.transform.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tab.transform.SetParent(OriginalParent.transform);
    }
}
