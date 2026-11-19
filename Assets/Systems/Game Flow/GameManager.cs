using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<MechanicUnlock> mechanicUnlocks;
    [SerializeField] private int maxWrong;
    public int _currentDay;
    public int _currentWrongGuesses;
    public int _daySaved;

    public static GameManager Instance;
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
    private void Start()
    {
        SaveDay();
    }

    public void UpdateMechanic()
    {
        foreach (var unlock in mechanicUnlocks)
        {
            bool isUnlocked = IsMechanicUnlocked(unlock);

            unlock.mechanicGO.SetActive(isUnlocked);
        }
    }

    private bool IsMechanicUnlocked(MechanicUnlock mechanic)
    {
        return _currentDay >= mechanic.unlockDay;
    }

    public void AddDay()
    {
        _currentDay++;
    }

    public void SaveDay()
    {
        _daySaved = _currentDay;
    }

    public bool GameOver()
    {
        if (_currentWrongGuesses < maxWrong)
            return false;
        else return true;
    }
}
