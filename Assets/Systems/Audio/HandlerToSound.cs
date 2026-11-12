using UnityEngine;
using UnityEngine.EventSystems;

public class HandlerToSound : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip[] clipClick;
    [SerializeField] private AudioClip[] clipDown;
    [SerializeField] private AudioClip[] clipUp;
    [SerializeField] private AudioClip[] clipMouseEnter;
    [SerializeField] private Transform spawnPos;
    [SerializeField, Range(0f, 1f)] private float volume;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayArraySound(clipClick);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayArraySound(clipDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayArraySound(clipUp);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayArraySound(clipMouseEnter);
    }


    private void PlayArraySound(AudioClip[] clips)
    {
        if (spawnPos == null)
            spawnPos = transform;

        if (clips.Length == 1)
            PlaySound(clips[0], spawnPos, volume);
        else if (clips.Length > 1)
            PlayRandomSound(clips, spawnPos, volume);
        else
            return;
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float v)
    {
        SoundFXManager.Instance.PlaySoundFXClip(audioClip, v, spawnTransform);
    }

    public void PlayRandomSound(AudioClip[] audioClip, Transform spawnTransform, float v)
    {
        SoundFXManager.Instance.PlayRandomSoundFXClip(audioClip, v, spawnTransform);
    }
}
