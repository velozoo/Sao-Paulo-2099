using Unity.VisualScripting;
using UnityEngine;

public enum TypesOfProblems
{
    None,
    Appearance,
    WrongDestination,
    DigitalFilliated,
    InconsistentCredential,
    InaccessibleDestination,
    LarpGram,
    NoEasyPass,
    MalfunctionEasyPass,
}

public class PassengerGenerator : MonoBehaviour
{
    [SerializeField] private TypewriterEffect typewriterEffect;
    private GameObject _currentGOPassenger;

    public void InstantiatePassenger(GenericPassengerData passenger)
    {
        if (_currentGOPassenger != null)
            Destroy(_currentGOPassenger);

        _currentGOPassenger = new GameObject();

        SpriteRenderer passengerSR = _currentGOPassenger.AddComponent<SpriteRenderer>();

        passengerSR.sprite = passenger.sprite;

        _currentGOPassenger.name = passenger.name;

        typewriterEffect.SetText(passengerSR.name);

        print("Nome: " + passenger.name + " " + passenger.lastName);
        print("Trabalho: " + passenger.job);
        print("Estação de Destino: " + passenger.destinyStation);
        print("Está aliada ao Sol Vermelho? " + passenger.isAllied);
        print("Problema: " + passenger.problem);
        print("Data de Nascimento: " + passenger.day + "/" + passenger.month + "/" + passenger.year);
    }
}
