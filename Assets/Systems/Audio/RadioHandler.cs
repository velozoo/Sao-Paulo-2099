using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum IconType
{
    Pause,
    Next
}

public class RadioHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text musicName;
    public TMP_Text pauseIcon;
    [SerializeField] private IconType buttonType;
    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    [Header("Text Anim")]
    [SerializeField] private RectTransform textRT;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 maxPos;
    [SerializeField] private float speed;
    [SerializeField] private float time;

    public bool canMove;

    private void Start()
    {
        MusicManager.Instance.PlayNextTrack();
        musicName.text = MusicManager.Instance.GetMusicName();
        ResetPosition();
        StartCoroutine(LoopReset());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundFXManager.Instance.PlaySoundFXClip(clickClip, volume);
        if (buttonType == IconType.Pause)
            PauseMusic();
        else if (buttonType == IconType.Next)
        {
            if (pauseIcon.text == "o")
            {
                pauseIcon.text = "||";
            }
            MusicManager.Instance.PlayNextTrack();
            musicName.text = MusicManager.Instance.GetMusicName();
            ResetPosition();
        }
    }

    private void PauseMusic()
    {
        if (pauseIcon.text == "o")
        {
            canMove = false;
            MusicManager.Instance.ResumeMusic();
            pauseIcon.text = "||";
        }
        else
        {
            canMove = true;
            MusicManager.Instance.PauseMusic();
            pauseIcon.text = "o";
        }
    }

    private void Update()
    {
        if(!canMove || buttonType == IconType.Pause)
            return;

        if (Vector3.Distance(textRT.anchoredPosition3D, maxPos) <= 0.1f)
        {
            ResetPosition();
            return;
        }

        MoveText();

    }

    public void MoveText()
    {
        textRT.anchoredPosition3D = Vector3.MoveTowards(textRT.anchoredPosition, maxPos, speed * Time.deltaTime);
    }


    private IEnumerator LoopReset()
    {
        yield return new WaitForSeconds(time);

        ResetPosition();

        StartCoroutine(LoopReset());
    }

    public void ResetPosition()
    {
        textRT.anchoredPosition3D = spawnPos;
    }

}
