using UnityEngine;

[CreateAssetMenu(fileName = "NewsData", menuName = "Scriptable Object/Daily/NewsData")]
public class NewsData : ScriptableObject
{
    public string headline;
    [TextArea(2, 10)]
    public string[] dialogueLines;
}
