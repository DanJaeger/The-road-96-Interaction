using System;
using UnityEngine;

/// <summary>
/// NPCStateManager controls the state of an NPC.
/// Responsibilities:
/// - Store NPC configuration (animations, audio, settings).
/// - Handle NPC state flags (interactable, UI canvas visibility).
/// - Provide access to components like Animator and AudioSource.
/// - Play dialogue audio clips.
/// </summary>
public class NPCStateManager : MonoBehaviour
{
    [Header("Components")]
    private Animator _animator;         // NPC animator reference
    private AudioSource _audioSource;   // NPC audio source (voice, SFX)

    [Header("NPC Settings")]
    [SerializeField] private NPC _npc;  // ScriptableObject with NPC data (name, clips, etc.)

    [Header("States")]
    private bool _canInteract;          // Whether the NPC can currently be interacted with
    private bool _canShowCanvas;        // Whether the NPC can show UI canvas (dialogue, etc.)
    private int _isSadAnimationHash;    // Cached hash for "IsSad" animator parameter

    #region Properties
    public Animator Animator => _animator;
    public NPC Npc { get => _npc; set => _npc = value; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanShowCanvas { get => _canShowCanvas; set => _canShowCanvas = value; }
    public int IsSadAnimationHash => _isSadAnimationHash;
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Cache components at start
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        // Cache animator parameter for faster access
        _isSadAnimationHash = Animator.StringToHash("IsSad");

        // Initialize states
        _canInteract = true;
        _canShowCanvas = false;
    }
    #endregion

    #region Audio
    /// <summary>
    /// Plays the NPC's dialogue audio clip by index.
    /// </summary>
    /// <param name="clipNumber">Index of the audio clip to play (from NPC data).</param>
    public void PlayAudio(int clipNumber)
    {
        if (_npc == null || _npc.ChatAudioClips == null || clipNumber < 0 || clipNumber >= _npc.ChatAudioClips.Length)
        {
            Debug.LogWarning($"[NPCStateManager] Invalid audio clip index: {clipNumber}");
            return;
        }

        _audioSource.clip = _npc.ChatAudioClips[clipNumber];
        _audioSource.Play();
    }
    #endregion
}
