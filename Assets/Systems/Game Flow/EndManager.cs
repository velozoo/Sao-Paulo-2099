using System.Threading.Tasks;
using System.Collections;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [Header("Ending")]
    [SerializeField] private string[] textEnd;
    [SerializeField] private string finalPhrase;
    [SerializeField] private TMPWriter finalTitle;
    [SerializeField] private TMP_Text finalText;
    [SerializeField] private float[] delayLines;

    [Header("Credits")]
    [SerializeField] private RectTransform credits;
    [SerializeField] private float creditsSpeed = 50f;
    [SerializeField] private float creditsEndY = 1500f;
    [SerializeField] private GameObject endScreen;

    

    public void StartEnding()
    {
        endScreen.SetActive(true);
        StartCoroutine(PlayEnding());
    }

    private IEnumerator PlayEnding()
    {
        finalText.text = textEnd[0];
        yield return new WaitForSeconds(delayLines[0]);
        finalText.text = textEnd[1];
        yield return new WaitForSeconds(delayLines[1]);
        finalText.text = textEnd[2];
        yield return new WaitForSeconds(delayLines[2]);
        finalText.text = textEnd[3];
        yield return new WaitForSeconds(delayLines[3]);
        finalText.text = textEnd[4];
        yield return new WaitForSeconds(delayLines[4]);
        finalText.text = textEnd[5];
        yield return new WaitForSeconds(delayLines[5]);

        finalText.text = finalPhrase;
        yield return new WaitForSeconds(delayLines[6]);
        finalText.text = " ";

        Task taskFadeOut = ScreenFader.Instance.FadeOut(2);

        yield return new WaitUntil(() => taskFadeOut.IsCompleted);

        yield return new WaitForSeconds(1);

        MusicManager.Instance.FadeInMusic(2);
        StartCoroutine(PlayCredits());
    }

    private IEnumerator PlayCredits()
    {
        while (credits.anchoredPosition.y < creditsEndY)
        {
            credits.anchoredPosition += Vector2.up * creditsSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        MusicManager.Instance.FadeOutMusic(2);

        Task taskFadeOut = ScreenFader.Instance.FadeOut(2);

        yield return new WaitUntil(() => taskFadeOut.IsCompleted);


        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Start Screen");
    }
}
