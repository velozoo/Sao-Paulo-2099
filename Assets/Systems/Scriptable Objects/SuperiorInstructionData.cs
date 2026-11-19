using UnityEngine;

[CreateAssetMenu(fileName = "InstructionData", menuName = "Scriptable Object/Daily/InstructionData")]
[System.Serializable]
public class EmailUnlock
{
    public SuperiorInstructionData emailData;
    public int unlockDay;
    public GameObject emailGO;
}

public class SuperiorInstructionData : ScriptableObject
{
    public string emailFrom;
    public string emailFor;
    public string emailSubject;

    [TextArea(2, 10)]
    public string emailContent;
}