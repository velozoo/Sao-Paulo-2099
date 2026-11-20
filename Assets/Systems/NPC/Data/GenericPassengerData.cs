using UnityEngine;

[CreateAssetMenu(fileName = "GenericPassengerData", menuName = "Scriptable Object/NPC/GenericData")]
public class GenericPassengerData : ScriptableObject
{
    public string name;
    public string lastName;
    public string job;
    public string destinyStation;
    public string textAllied;
    public bool isAllied;
    public TypesOfProblems problem; 

    [Header("Birth Dates")]
    public int day;
    public int month;
    public int year;

    [Header("Dialogue Lines")]
    public string entryLine;
    public string avoidLine;
    public string passLine;
    public AudioClip[] voice;

    public Sprite sprite;

    public LarpGramData larpGramData;

    public EasyPassData easyPassData;
}
