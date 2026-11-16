using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        canvasGroup.alpha = 1;
    }

    async Task Fade(float targetTransperancy, float fadeDuration)
    {
        float start = canvasGroup.alpha, t = 0;
        while (t< fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransperancy, t / fadeDuration);
            await Task.Yield();
        }
        canvasGroup.alpha = targetTransperancy;
    }

    public async Task FadeOut(float f)
    {
        canvasGroup.alpha = 0;
        await Fade(1, f);
    }
    public async Task FadeIn(float f)
    {
        canvasGroup.alpha = 1;
        await Fade(0, f);
    }
}
