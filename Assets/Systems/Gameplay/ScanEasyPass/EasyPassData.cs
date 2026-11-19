using UnityEngine;

[CreateAssetMenu(fileName = "EasyPassData", menuName = "Scriptable Object/NPC/EasyPassData")]
public class EasyPassData : ScriptableObject
{
    public string expirationDate;
    public string validSubscription;
    public bool hasPass;
}
