using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LarpGramPostGO : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image profilePic;
    [SerializeField] private Image postImage;
    [SerializeField] private TMP_Text likesText;
    [SerializeField] private TMP_Text commentsText;
    [SerializeField] private TMP_Text descriptionText;

    public void SetPost(string name, Sprite profilePict, Sprite postImg, 
                        string likes, string comments, string description)
    {
        nameText.text = name;
        profilePic.sprite = profilePict;
        postImage.sprite = postImg;
        likesText.text = likes;
        commentsText.text = comments;
        descriptionText.text = description;
    }
}
