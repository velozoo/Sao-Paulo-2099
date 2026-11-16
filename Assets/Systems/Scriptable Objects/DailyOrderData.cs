using UnityEngine;

[CreateAssetMenu(fileName = "DailyOrderData", menuName = "Scriptable Object/Daily/DailyOrderData")]
public class DailyOrderData : ScriptableObject
{
    [TextArea(2, 10)]
    public string[] order;
}
