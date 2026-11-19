using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmailBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EmailManager emailManager;
    [SerializeField] private SuperiorInstructionData emailData;
    [SerializeField] private Image boxImage;
    [SerializeField] private Color viewedColor;


    public void OnPointerClick(PointerEventData eventData)
    {
        boxImage.color = viewedColor;
        emailManager.SetEmail(emailData);
    }
}
