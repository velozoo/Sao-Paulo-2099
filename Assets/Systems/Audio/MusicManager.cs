using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            Instance = this;
        }
    }

    IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration)
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
    }
}
