using UnityEngine;

public class PassengerValidator : MonoBehaviour
{
    public void Pass()
    {
        if (PassengerShouldPass() == true)
            print("_______________________acertou_______________________");
        else
            print("_______________________errou_______________________");

        GameManager.Instance.SetPassenger();
    }

    public void NotPass()
    {
        if (PassengerShouldPass() == false)
            print("_______________________acertou_______________________");
        else
            print("_______________________errou_______________________");

        GameManager.Instance.SetPassenger();
    }

    public bool PassengerShouldPass()
    {
        TypesOfProblems problem = GameManager.Instance._currentPassenger.problem;

        if (problem == TypesOfProblems.None)
            return true;

        return problem switch
        {
            TypesOfProblems.Appearance => false,
            TypesOfProblems.WrongDestination => false,
            TypesOfProblems.DigitalFilliated => false,
            TypesOfProblems.InconsistentCredential => false,
            TypesOfProblems.InaccessibleDestination => false,
            TypesOfProblems.LarpGram => false,
            TypesOfProblems.NoEasyPass => false,
            TypesOfProblems.MalfunctionEasyPass => false,
            _ => true,
        };
    }
}
