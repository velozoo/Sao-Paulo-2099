using UnityEngine;

public class PassengerValidator : MonoBehaviour
{
    public void Pass()
    {
        if (PassengerShouldPass() == true && GameManager.Instance._passengerIsTransitioning != true)
            print("_______________________acertou_______________________");
        else
            print("_______________________errou_______________________");

        GameManager.Instance.NextPassenger(false);
    }

    public void NotPass()
    {
        if (PassengerShouldPass() == false && GameManager.Instance._passengerIsTransitioning != true)
            print("_______________________acertou_______________________");
        else
            print("_______________________errou_______________________");

        GameManager.Instance.NextPassenger(true);
    }

    public bool PassengerShouldPass()
    {
        TypesOfProblems problem = GameManager.Instance._currentPassenger.problem;

        if (problem == TypesOfProblems.None)
            return true;

        return problem switch
        {
            TypesOfProblems.Appearance => false, // vai ter algum adereço do partido // NPC NO MONITOR
            TypesOfProblems.WrongDestination => false, // tem que ver se tem no mapa // MAPA
            TypesOfProblems.DigitalFilliated => false, // ver se esta filiado na carteira digital // DOCUMENTO PC ?
            TypesOfProblems.InconsistentCredential => false, // data, nome ou algo errado no seu documento digital // DOCUMENTO PC ?
            TypesOfProblems.InaccessibleDestination => false, // se for dada como inacessivel n vai poder passar // MAPA + ANUNCIO NO NOTICARIO
            TypesOfProblems.LarpGram => false, // ver se tem alguma menção no larpgram // LARPGRAM PC
            TypesOfProblems.NoEasyPass => false,// ta sem chip // SCAN MONITOR DOCUMENTO PC
            TypesOfProblems.MalfunctionEasyPass => false, // chip n funciona // SCAN MONITOR DOCUMENTO PC
            _ => true,
        };
    }
}
