using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private NewsManager newsManager;
    [SerializeField] private EmailManager emailManager;
    [SerializeField] private PassengerManager passengerManager;
    [SerializeField] private DesktopScreen desktopScreen;
    [SerializeField] private NPCSpawner npcSpawner;

    [Header("Delay Before Each Event")]
    [SerializeField] private float newsDelay;
    [SerializeField] private float emailDelay;
    [SerializeField] private float passengerDelay;

    [Header("Clock Out")]
    [SerializeField] private Button clockOutButton;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float blackScreenDuration;

    [Header("Debug")]
    [SerializeField] private bool doNews;


    private bool _newsFinished;
    private bool _passengersFinished;

    private Coroutine _dayCoroutine;

    public static DayManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
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

        desktopScreen._hasOpenToday = false;
        _newsFinished = false;
        _passengersFinished = false;
        clockOutButton.interactable = false;

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

        emailManager.StartEmail();

        yield return new WaitForSeconds(passengerDelay);

        passengerManager.StartPassenger(OnPassengerFinished);

        yield return new WaitUntil(() => _passengersFinished);
        _dayCoroutine = null;
    }

    private void OnNewsFinished()
    {
        _newsFinished = true;
    }

    private void OnPassengerFinished()
    {
        _passengersFinished = true;
        npcSpawner.npcSpawn = false;
        clockOutButton.interactable = true;
    }

    public async void ClockOut()
    {
        if (!_passengersFinished)
            return;

        clockOutButton.interactable = false;

        await ScreenFader.Instance.FadeTransition(
            fadeOutDuration,
            fadeInDuration,
            blackScreenDuration,

            () =>
            {
                DiaryManager.Instance.ToggleDiary();
                return Task.CompletedTask;
            }
        );
    }
}
