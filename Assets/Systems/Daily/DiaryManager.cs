using System.Threading.Tasks;
using System.Collections.Generic;
using TMPEffects.Components;
using TMPro;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private DiaryData[] diaryData;

    [Header("TMP")]
    [SerializeField] private TMP_Text dayTitle;
    [SerializeField] private TMP_Text diaryText;
    [SerializeField] private TMPWriter diaryWriter;

    [Header("Others")]
    [SerializeField] private GameObject diaryScreen;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;

    private List<DiaryData> _availableDiaryData = new List<DiaryData>();

    private List<string> _pages = new List<string>();

    private int _currentPage;
    private int _currentEntry;
    private int _currentDay;

    private string _currentTitle;

    public static DiaryManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ToggleDiary()
    {
        _currentDay = GameManager.Instance._currentDay - 1;

        _availableDiaryData.Clear();

        for (int i = 0; i <= _currentDay && i < diaryData.Length; i++)
        {
            _availableDiaryData.Add(diaryData[i]);
        }

        if (_availableDiaryData.Count == 0)
        {
            Debug.LogWarning("Nenhuma entrada disponível no diário.");
            return;
        }

        LoadEntry(_availableDiaryData.Count - 1);

        diaryScreen.SetActive(true);
    }

    private void LoadEntry(int entryIndex)
    {
        if (entryIndex < 0 || entryIndex >= _availableDiaryData.Count)
            return;

        _currentEntry = entryIndex;

        GeneratePages(_availableDiaryData[_currentEntry]);
    }

    private void GeneratePages(DiaryData data)
    {
        _pages.Clear();

        _currentTitle = data.dayTitle;

        string currentPage = "";

        foreach (string paragraph in data.paragraphs)
        {
            string testText = currentPage;

            if (!string.IsNullOrEmpty(testText))
                testText += "\n\n";

            testText += paragraph;

            diaryText.text = testText;
            diaryText.ForceMeshUpdate();

            if (diaryText.textInfo.pageCount > 1)
            {
                if (!string.IsNullOrEmpty(currentPage))
                {
                    _pages.Add(currentPage);
                    currentPage = paragraph;
                }
                else
                {
                    currentPage = paragraph;
                }
            }
            else
            {
                currentPage = testText;
            }
        }

        if (!string.IsNullOrEmpty(currentPage))
            _pages.Add(currentPage);

        _currentPage = 0;

        DisplayPage();
    }

    private void DisplayPage()
    {
        dayTitle.text = _currentPage == 0 ? _currentTitle : "";

        diaryText.text = _pages[_currentPage];

        bool isLastPage = _currentPage == _pages.Count - 1;
        bool isLastEntry = _currentEntry == _availableDiaryData.Count - 1;

        finishButton.SetActive(isLastPage && isLastEntry);
    }

    public void NextPage()
    {
        if (_currentPage < _pages.Count - 1)
        {
            _currentPage++;
            DisplayPage();
        }
        else if (_currentEntry < _availableDiaryData.Count - 1)
        {
            LoadEntry(_currentEntry + 1);
        }
    }

    public void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            DisplayPage();
        }
        else if (_currentEntry > 0)
        {
            LoadEntry(_currentEntry - 1);
            _currentPage = _pages.Count - 1;
            DisplayPage();
        }
    }

    public async void FinishDiary()
    {
        await ScreenFader.Instance.FadeOut(fadeOutDuration);

        GameManager.Instance.AddDay();
        DayManager.Instance.StartDay();
        diaryScreen.SetActive(false);
    }
}
