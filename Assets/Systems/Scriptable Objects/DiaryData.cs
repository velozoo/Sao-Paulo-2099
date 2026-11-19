using UnityEngine;

[CreateAssetMenu(fileName = "DiaryData", menuName = "Scriptable Object/Daily/DiaryData")]
[System.Serializable]
public class DiaryDay
{
    public DiaryData[] diaryDatas;
}

public class DiaryData : ScriptableObject
{
    public string dayTitle = " ";
    [TextArea(5, 20)]
    public string pageContent;
}