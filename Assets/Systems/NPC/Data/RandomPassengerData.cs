using UnityEngine;

[CreateAssetMenu(fileName = "RandomPassengerData", menuName = "Scriptable Object/NPC/RandomData")]
public class RandomPassengerData : ScriptableObject
{
    public string[] name;
    public string[] lastName;
    public string[] state;
    public string[] station;
    public bool isAllied;

    [Header("Birth Dates")]
    //DAY
    public int minDay;
    public int maxDay;
    //MONTH
    public int minMonth, maxMonth;
    //YEAR
    public int minYear, maxYear;

    
}
