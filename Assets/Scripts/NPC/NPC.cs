using UnityEngine;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class NPC : ScriptableObject
{
    public string NPCName;
    public AudioClip[] ChatAudioClips;
    public string[] ChatAnimations;

}
