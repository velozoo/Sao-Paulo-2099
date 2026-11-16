using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LarpGramData", menuName = "Scriptable Object/NPC/LarpGramData")]
public class LarpGramData : ScriptableObject
{
    public Sprite profilePicture;
    public string nickname;
    public string numberPosts;
    public string followers;
    public List<LarpGramPost> posts;
}
