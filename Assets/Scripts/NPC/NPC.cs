using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC Files Archive")]
public class NPC : ScriptableObject
{
    public string NPCName;
    [TextArea(3, 15)]
    public string[] Dialogue;
    [TextArea(3, 15)]
    public string[] ChatOptions;
    public AudioClip[] ChatAudioClips;
    
}
