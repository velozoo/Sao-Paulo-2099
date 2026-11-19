using System.Threading.Tasks;
using System.Collections.Generic;
using TMPEffects.Components;
using TMPro;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<DiaryDay> diaryDays;

    [Header("TMP")]
    [SerializeField] private TMP_Text dayTitle;
    [SerializeField] private TMP_Text diaryText;
    [SerializeField] private TMPWriter diaryWriter;


    [Header("Fade Transition")]
    [SerializeField] private float fadeInD;
    [SerializeField] private float fadeOutD;
    [SerializeField] private float blackScreenD;

    [Header("Others")]
    [SerializeField] private GameObject diaryScreen;
    [SerializeField] private GameObject finishButton;

    private List<DiaryData> _currentPages = new List<DiaryData>();
    private int _currentDay;
    private int _currentPage;


    public static DiaryManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdatePages();
    }

    private void UpdatePages()
    {
        _currentDay = GameManager.Instance._currentDay - 1;
        GameManager.Instance.SaveDay();

        finishButton.SetActive(false);

        _currentPages.Clear();

        for(int i = 0; i <= _currentDay; i++)
        {
            _currentPages.AddRange(diaryDays[i].diaryDatas);
        }
        _currentPage = _currentPages.Count - 2;

        DisplayPage(false);
    }
    private void DisplayPage(bool skipText)
    {
        if (_currentPage < 0 || _currentPage >= _currentPages.Count)
            return;

        dayTitle.text = _currentPages[_currentPage].dayTitle;
        diaryText.text = _currentPages[_currentPage].pageContent;

        diaryWriter.StartWriter();
        

        finishButton.SetActive(_currentPage == _currentPages.Count - 1);
    }

    public void ToggleDiary()
    {
        diaryScreen.SetActive(true);
        UpdatePages();
    }
    
    public void NextPage()
    {
        if (_currentPage < _currentPages.Count -1)
        {
            _currentPage++;
            DisplayPage(false);
        }
    }

    public void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            DisplayPage(true);
        }
    }

    public async void FinishDay()
    {
        await ScreenFader.Instance.FadeTransition(
            fadeOutD,
            fadeInD,
            blackScreenD,

            () =>
            {
                GameManager.Instance.AddDay();
                DayManager.Instance.StartDay();
                diaryScreen.SetActive(false);
                return Task.CompletedTask;
            }
        );
    }
}
