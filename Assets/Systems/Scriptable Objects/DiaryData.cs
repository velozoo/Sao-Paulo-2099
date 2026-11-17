using UnityEngine;

[CreateAssetMenu(fileName = "DiaryData", menuName = "Scriptable Object/Daily/DiaryData")]
public class DiaryData : ScriptableObject
{
    public string dayTitle;
    [TextArea(5, 20)]
    public string[] paragraphs;

    /*public string dayTitle = " ";
    [TextArea(5, 20)]
    public string pageContent;*/
}
