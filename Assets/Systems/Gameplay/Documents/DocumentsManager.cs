using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DocumentsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text jobText;
    [SerializeField] private TMP_Text birthDateText;
    [SerializeField] private TMP_Text destinationText;
    [SerializeField] private TMP_Text affiliatedText;

    public static DocumentsManager Instance;


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
    public void UpdateDocuments(GenericPassengerData passenger)
    {
        nameText.text = "Nome: " + passenger.name + " " + passenger.lastName;
        jobText.text = "Job: " + passenger.job;
        birthDateText.text = "Data de Nascimento: " + passenger.day + "/" + passenger.month + "/" + passenger.year;
        destinationText.text = "Estação de Destino: " + passenger.destinyStation;
        affiliatedText.text = "Está aliada ao Sol Vermelho? " + passenger.isAllied;
    }
}
