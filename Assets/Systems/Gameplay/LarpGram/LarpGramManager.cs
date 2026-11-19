using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LarpGramManager : MonoBehaviour
{
    [SerializeField] private Image profilePic;
    [SerializeField] private TMP_Text postText;
    [SerializeField] private TMP_Text followerText;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject postPrefab;
    [SerializeField] private GameObject postSectionParent;

    public static LarpGramManager Instance;

    private GameObject _postGO;

    private void Awake()
    {
        if(Instance == null) 
            Instance = this;
        else
        {
            Destroy(gameObject);
            Instance = this;
        }
    }

    public void UpdateLarp(GenericPassengerData passenger)
    {
        LarpGramData passengerData = passenger.larpGramData;

        string fullName = passenger.name + " " + passenger.lastName;

        profilePic.sprite = passengerData.profilePicture;
        postText.text = passengerData.posts.Count.ToString();
        followerText.text = passengerData.followers;
        nicknameText.text = passengerData.nickname;
        nameText.text = fullName;

        if (passenger.larpGramData.posts.Count == 0)
            return;

        _postGO = Instantiate(postPrefab, postSectionParent.transform.parent);

        LarpGramPostGO post = _postGO.GetComponent<LarpGramPostGO>();

        foreach (LarpGramPost postI in passengerData.posts)
        {
            post.SetPost(fullName, passengerData.profilePicture, postI.postImage, 
                postI.likes, postI.commentaries, postI.description);
        }
    }

    public void DestroyPost()
    {
        if (_postGO != null)
        Destroy(_postGO);
    }
}
