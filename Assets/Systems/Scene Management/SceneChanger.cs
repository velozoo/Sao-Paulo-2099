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

    private bool _inTransition;
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && anyClick && !_inTransition)
            DoFade();
    }

    public async Task DoFade()
    {
        if (_inTransition)
            return;
        _inTransition = true;

        await ScreenFader.Instance.FadeOutTransition(fadeOut, blackScreenTime, () =>
        {
            SceneManager.LoadScene(sceneName); 
            return Task.CompletedTask;
        });
    }
}
