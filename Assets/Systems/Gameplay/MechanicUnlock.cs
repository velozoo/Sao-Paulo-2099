using UnityEngine;

public enum MechanicType
{
    Map,
    DigitalDocuments,
    LarpGram,
    EasyPassScanner,
}

[System.Serializable]
public class MechanicUnlock
{
    public MechanicType mechanicType;
    public int unlockDay;
    public GameObject mechanicGO;
}
