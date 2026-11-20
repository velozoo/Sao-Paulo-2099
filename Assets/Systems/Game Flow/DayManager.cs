using System.Collections;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private NewsManager newsManager;
    [SerializeField] private EmailManager emailManager;
    [SerializeField] private PassengerManager passengerManager;
    [SerializeField] private DesktopScreen desktopScreen;
    [SerializeField] private NPCSpawner npcSpawner;
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private EndManager endManager;

    [Header("Delay Before Each Event")]
    [SerializeField] private float newsDelay;
    [SerializeField] private float emailDelay;
    [SerializeField] private float passengerDelay;

    [Header("Clock Out")]
    [SerializeField] private Button clockOutButton;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float blackScreenDuration;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private AudioClip alarmClip;
    [SerializeField, Range(0f, 1f)] private float volume;

    [Header("Inspector Sprites")]
    [SerializeField] private Image inspectorImage;
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite tenseSprite;
    [SerializeField] private Sprite sadSprite;

    [Header("Debug")]
    [SerializeField] private bool doNews;


    private bool _newsFinished;
    private bool _passengersFinished;

    private Coroutine _dayCoroutine;

    public static DayManager Instance;

    private bool onStart;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        onStart = true;
        if (onStart)
        {
            GameManager.Instance.RestartGame();
            onStart = false;
        }
        StartDay();
        FadeOnce();
    }

    public async void FadeOnce()
    {
        await ScreenFader.Instance.FadeIn(fadeInDuration);
    }

    public void StartDay()
    {
        if (_dayCoroutine != null)
            return;

        _dayCoroutine = StartCoroutine(DaySequence());
    }

    private IEnumerator DaySequence()
    {
        GameManager.Instance.UpdateMechanic();
        GameManager.Instance.SaveDay();
        UpdateInspectorSprite();

        desktopScreen._hasOpenToday = false;
        _newsFinished = false;
        _passengersFinished = false;
        clockOutButton.interactable = false;
        LarpGramManager.Instance.ResetLarpGram();

        MusicManager.Instance.FadeInAmbient(fadeInDuration, ambientSound);

        yield return new WaitForSeconds(blackScreenDuration);


        if (doNews)
        {
            yield return new WaitForSeconds(newsDelay);

            newsManager.BeginNews(OnNewsFinished);

            yield return new WaitUntil(() => _newsFinished);
        }

        yield return new WaitForSeconds(emailDelay);

        npcSpawner.npcSpawn = true;
        npcSpawner.StartSpawnNpc();
        //

        emailManager.StartEmail();

        yield return new WaitForSeconds(passengerDelay);

        passengerManager.StartPassenger(OnPassengerFinished);

        yield return new WaitUntil(() => _passengersFinished);
        _dayCoroutine = null;
    }

    private void UpdateInspectorSprite()
    {
        int currentDay = GameManager.Instance._currentDay;
        if(currentDay == 1 || currentDay == 2 || currentDay == 3)
        {
            inspectorImage.sprite = neutralSprite;
        }
        else if (currentDay == 4 || currentDay == 5)
        {
            inspectorImage.sprite = tenseSprite;
        }
        else
        {
            inspectorImage.sprite = sadSprite;
        }

    }

    private void OnNewsFinished()
    {
        _newsFinished = true;
    }

    private void OnPassengerFinished()
    {
        SoundFXManager.Instance.PlaySoundFXClip(alarmClip, volume);
        _passengersFinished = true;
        npcSpawner.npcSpawn = false;
        clockOutButton.interactable = true;
    }

    public async void ClockOut()
    {
        if (!_passengersFinished)
            return;

        clockOutButton.interactable = false;
        MusicManager.Instance.FadeOutMusic(fadeOutDuration);
        MusicManager.Instance.FadeOutAmbient(fadeInDuration);

        await ScreenFader.Instance.FadeTransition(
            fadeOutDuration,
            fadeInDuration,
            blackScreenDuration,

            () =>
            {
                if (GameManager.Instance.GameFinished())
                {
                    endManager.StartEnding();
                }
                else if (GameManager.Instance.GameOver())
                {
                    gameOverManager.ResetAll();
                    gameOverManager.DoGameOver();
                }
                else 
                {
                    DiaryManager.Instance.ToggleDiary();
                }

                return Task.CompletedTask;
            }
        );
    }

    public  void RetryDay()
    {
        StartCoroutine(RetrySequence());
    }

    private IEnumerator RetrySequence()
    {
        GameManager.Instance.LoadDay();

        Task taskFadeOut = ScreenFader.Instance.FadeOut(fadeOutDuration);

        yield return new WaitUntil(() => taskFadeOut.IsCompleted);

        yield return new WaitForSeconds(blackScreenDuration / 2);
        gameOverManager.ResetAll();
        StartDay();
        yield return new WaitForSeconds(blackScreenDuration / 2);

        Task taskFadeIn = ScreenFader.Instance.FadeIn(fadeInDuration);

        yield return new WaitUntil(() => taskFadeIn.IsCompleted);
    }
    public  void RestartGame()
    {
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        GameManager.Instance.LoadDay();

        Task taskFadeOut = ScreenFader.Instance.FadeOut(fadeOutDuration);

        yield return new WaitUntil(() => taskFadeOut.IsCompleted);

        yield return new WaitForSeconds(blackScreenDuration);
        gameOverManager.ResetAll();
        SceneManager.LoadScene("Start Screen");
    }
}
