using System.Collections;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    public int currentTrackIndex = 0;

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

    public string GetMusicName()
    {
        return musicLibrary.tracks[currentTrackIndex].trackName;
    }

    IEnumerator AnimateMusicCrossFade(AudioClip nextTrack, float fadeDuration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }

        musicSource.volume = 1f;
    }
    

    public void PlayNextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= musicLibrary.tracks.Length)
            currentTrackIndex = 0;

        if (!musicSource.isPlaying)
            ResumeMusic();

        StartCoroutine(AnimateMusicCrossFade(
            musicLibrary.tracks[currentTrackIndex].clip,
            1f
        ));
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
