using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PassengerGenerator passengerGenerator;

    [Header("NPCs per day")]
    [SerializeField] private List<PassengerDay> days;

    private int _currentDay;
    [HideInInspector] public GenericPassengerData _currentPassenger;
    private List<PassengerDay> _days;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    private void Start()
    {
        _days = new List<PassengerDay>();

        foreach (PassengerDay day in days)
        {
            PassengerDay dayCopy = new PassengerDay();
            dayCopy.passengers = new List<GenericPassengerData>(day.passengers);

            _days.Add(dayCopy);
        }

        _currentDay = 0;

        SetPassenger();
    }

    public void SetPassenger()
    {
        if (_days[_currentDay].passengers.Count == 0)
            _currentDay++;

        int passengerIndex = Random.Range(0, _days[_currentDay].passengers.Count);

        _currentPassenger = _days[_currentDay].passengers[passengerIndex];

        passengerGenerator.InstantiatePassenger(_currentPassenger);

        _days[_currentDay].passengers.RemoveAt(passengerIndex);
    }
}
