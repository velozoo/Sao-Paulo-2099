using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<MechanicUnlock> mechanicUnlocks;
    public int _currentDay; //encapsulamento

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
}
