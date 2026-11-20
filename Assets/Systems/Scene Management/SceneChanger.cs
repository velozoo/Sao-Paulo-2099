using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool anyClick;
    [SerializeField] private float fadeIn;
    [SerializeField] private float fadeOut;
    [SerializeField] private float blackScreenTime;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float volume;

    private bool _inTransition;
    private void Start()
    {
        FadeIn();
    }

    public async void FadeIn()
    {
        await ScreenFader.Instance.FadeIn(fadeOut);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && anyClick && !_inTransition)
            DoFade();
    }

    public async Task DoFade()
    {
        if (_inTransition)
            return;

        StartCoroutine(FadeAmbientVolume(0f, fadeOut));
        _inTransition = true;

        await ScreenFader.Instance.FadeOutTransition(fadeOut, blackScreenTime, () =>
        {
            SceneManager.LoadScene(sceneName); 
            return Task.CompletedTask;
        });
    }

    private IEnumerator FadeAmbientVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsedTime / duration
            );

            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
