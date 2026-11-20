using System.Collections;
using System.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private RadioHandler radioHandler;

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

    public void FadeOutMusic(float duration)
    {
        radioHandler.pauseIcon.text = "0";
        StartCoroutine(FadeVolume(0f, duration));
    }

    public void FadeInMusic(float duration)
    {
        musicSource.volume = 0f;
        musicSource.Play();
        radioHandler.pauseIcon.text = "||";

        StartCoroutine(FadeVolume(1f, duration));
    }

    private IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsedTime / duration
            );

            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public void FadeOutAmbient(float duration)
    {
        StartCoroutine(FadeAmbientVolume(0f, duration));
    }

    public void FadeInAmbient(float duration, AudioClip clip)
    {
        ambientSource.volume = 0f;
        ambientSource.clip = clip;
        ambientSource.Play();

        StartCoroutine(FadeAmbientVolume(0.15f, duration));
    }
    private IEnumerator FadeAmbientVolume(float targetVolume, float duration)
    {
        float startVolume = ambientSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            ambientSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsedTime / duration
            );

            yield return null;
        }

        ambientSource.volume = targetVolume;
    }
}
