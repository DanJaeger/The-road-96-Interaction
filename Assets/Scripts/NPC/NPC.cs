using UnityEngine;

/// <summary>
/// NPC ScriptableObject that stores configuration data for an NPC.
/// Responsibilities:
/// - Holds the NPC's display name.
/// - Stores dialogue audio clips (voice lines).
/// - Stores animation trigger names related to dialogue or states.
/// </summary>
[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/Create New NPC")]
public class NPC : ScriptableObject
{
    [Header("NPC Settings")]
    [Tooltip("The display name of the NPC (used in dialogues).")]
    public string NPCName;

    [Header("Dialogue Audio")]
    [Tooltip("Audio clips used when this NPC speaks.")]
    public AudioClip[] ChatAudioClips;

    [Header("Dialogue Animations")]
    [Tooltip("Animation state or trigger names associated with dialogue lines.")]
    public string[] ChatAnimations;
}
