using System.Collections.Generic;
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

    private List<GameObject> _postsGO = new List<GameObject>();

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

    private void Start()
    {
        ResetLarpGram();
    }

    public void UpdateLarp(GenericPassengerData passenger)
    {
        ResetLarpGram();

        LarpGramData passengerData = passenger.larpGramData;

        string fullName = passenger.name + " " + passenger.lastName;

        profilePic.sprite = passengerData.profilePicture;
        postText.text = passengerData.posts.Count.ToString();
        followerText.text = passengerData.followers;
        nicknameText.text = passengerData.nickname;
        nameText.text = fullName;

        if (passenger.larpGramData.posts.Count == 0)
            return;


        foreach (LarpGramPost postI in passengerData.posts)
        {
            GameObject postGO = Instantiate(
                postPrefab,
                postSectionParent.transform
            );

            _postsGO.Add(postGO);

            LarpGramPostGO post = postGO.GetComponent<LarpGramPostGO>();

            post.SetPost(
                fullName,
                passengerData.profilePicture,
                postI.postImage,
                postI.likes,
                postI.commentaries,
                postI.description
            );
        }
    }

    public void ResetLarpGram()
    {
        if( _postsGO.Count > 0 )
        DestroyPost();

        postText.text = "0";
        followerText.text = "0";
        nicknameText.text = "Nickname";
        nameText.text = "Nome";
    }

    public void DestroyPost()
    {
        foreach (GameObject post in _postsGO)
        {
            if (post != null)
                Destroy(post);
        }

        _postsGO.Clear();
    }
}
