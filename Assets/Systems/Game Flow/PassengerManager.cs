using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PassengerManager : MonoBehaviour
{
    [SerializeField] private Button clockOutButton;

    [Header("References")]
    [SerializeField] private PassengerGenerator passengerGenerator;

    [Header("NPCs per day")]
    [SerializeField] private List<PassengerDay> days;
    [SerializeField] private float waitBetweenPassenger;
    [SerializeField] private float minWait; 
    [SerializeField] private GenericPassengerData finalPassenger;
    [SerializeField] private int specialPassengerDay = 7;

    public GenericPassengerData _currentPassenger;
    [HideInInspector] public bool _passengerIsTransitioning;

    private List<PassengerDay> _days;
    private int _currentDay;

    private Action _onPassengersFinished;

    private bool _hasActivePassenger;
    private bool _passengersFinished;
    public bool CanMakeDecision =>
    _hasActivePassenger &&
    !_passengerIsTransitioning &&
    !_passengersFinished;

    public static PassengerManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartPassenger(Action onComplete)
    {
        _onPassengersFinished = onComplete;

        _currentDay = GameManager.Instance._currentDay - 1;

        _days = new List<PassengerDay>();

        if (days == null)
        {
            Debug.LogError("A lista Days não foi configurada!");
            FinishPassengers();
            return;
        }

        foreach (PassengerDay day in days)
        {
            PassengerDay dayCopy = new PassengerDay();
            dayCopy.passengers = new List<GenericPassengerData>(day.passengers);

            _days.Add(dayCopy);
        }

        if (_days == null || _days.Count == 0)
        {
            Debug.LogError("Nenhum dia de passageiros foi configurado!");
            FinishPassengers();
            return;
        }

        if (_currentDay < 0 || _currentDay >= _days.Count)
        {
            Debug.LogError("Índice do dia inválido!");
            FinishPassengers();
            return;
        }

        if (_days[_currentDay].passengers == null)
        {
            Debug.LogError("A lista de passageiros deste dia não foi configurada!");
            FinishPassengers();
            return;
        }

        _hasActivePassenger = false;
        _passengerIsTransitioning = false;
        _passengersFinished = false;

        print("toggle1");

        ToggleClockOut(true);
    }

    private bool HasPassengersRemaining()
    {
        return _currentDay >= 0 &&
               _currentDay < _days.Count &&
               _days[_currentDay].passengers.Count > 0;
    }

    private void SetPassenger()
    {
        if (!HasPassengersRemaining())
        {
            FinishPassengers();
            return;
        }

        _passengerIsTransitioning = true;

        int passengerIndex;

        bool isSpecialDay = _currentDay == specialPassengerDay - 1;

        if (isSpecialDay &&
            _days[_currentDay].passengers.Count > 1)
        {
            List<GenericPassengerData> availablePassengers =
                _days[_currentDay].passengers.FindAll(
                    p => p != finalPassenger
                );

            passengerIndex = _days[_currentDay].passengers.IndexOf(
                availablePassengers[UnityEngine.Random.Range(
                    0,
                    availablePassengers.Count
                )]
            );
        }
        else
        {
            passengerIndex = UnityEngine.Random.Range(
                0,
                _days[_currentDay].passengers.Count
            );
        }

        _currentPassenger = _days[_currentDay].passengers[passengerIndex];

        _days[_currentDay].passengers.RemoveAt(passengerIndex);

        passengerGenerator.InstantiatePassenger(_currentPassenger);

        passengerGenerator.MoveInPassenger(() =>
        {
            _passengerIsTransitioning = false;
            _hasActivePassenger = true;
            print("pront po ir");
        });
    }

    public void NextPassenger(bool isAvoided)
    {
        if (_passengerIsTransitioning || !_hasActivePassenger)
            return;

        _hasActivePassenger = false;
        _passengerIsTransitioning = true;

        if (!isAvoided)
            passengerGenerator.MoveOutPassenger(CheckPassengersFinished);
        else
            passengerGenerator.MoveAvoidPassenger(CheckPassengersFinished);
    }

    private void CheckPassengersFinished()
    {
        _passengerIsTransitioning = false;

        if (HasPassengersRemaining())
        {
            StartCoroutine(PassengersCoolDown());
            //ToggleCallNext(true);
        }
        else
            FinishPassengers();
    }

    private void FinishPassengers()
    {
        if (_passengersFinished)
            return;

        _passengersFinished = true;
        _passengerIsTransitioning = false;
        _hasActivePassenger = false;

        //ToggleCallNext(false);

        _onPassengersFinished?.Invoke();
        _onPassengersFinished = null;
    }

    public void CallNextPassenger()
    {
        if (_passengerIsTransitioning ||
            _hasActivePassenger ||
            _passengersFinished)
            return;

        ToggleClockOut(false);

        SetPassenger();
    }

    private IEnumerator PassengersCoolDown()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, waitBetweenPassenger));
        CallNextPassenger();
    }

    public void ToggleClockOut(bool b)
    {
        clockOutButton.interactable = b;
    }

}
