using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PassengerGenerator passengerGenerator;

    [Header("NPCs per day")]
    [SerializeField] private List<PassengerDay> days;

    public int _currentDay;
    [HideInInspector] public GenericPassengerData _currentPassenger;
    private List<PassengerDay> _days;
    [HideInInspector] public bool _passengerIsTransitioning;

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

        //_currentDay = 0;

        SetPassenger();
    }

    private void SetPassenger()
    {
        if (_currentDay >= _days.Count)
        {
            _passengerIsTransitioning = false;
            return;
        }

        if (_days[_currentDay].passengers.Count == 0)
        {
           _currentDay++;

            if (_currentDay >= _days.Count)
            {
                _passengerIsTransitioning = false;
                return;
            }
        }

        int passengerIndex = Random.Range(0, _days[_currentDay].passengers.Count);

        _currentPassenger = _days[_currentDay].passengers[passengerIndex];

        _days[_currentDay].passengers.RemoveAt(passengerIndex);

        passengerGenerator.InstantiatePassenger(_currentPassenger);

        passengerGenerator.MoveInPassenger(() => _passengerIsTransitioning = false);
    }

    public void NextPassenger(bool isAvoided)
    {
        if (_passengerIsTransitioning) return;

        _passengerIsTransitioning = true;

        if (!isAvoided)
        {
            passengerGenerator.MoveOutPassenger(() =>
            {
                SetPassenger();
            });
        }
        else
        {
            passengerGenerator.MoveAvoidPassenger(() =>
            {
                SetPassenger();
            });
        }
    }
}
