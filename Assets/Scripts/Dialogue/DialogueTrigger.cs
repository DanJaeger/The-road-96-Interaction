using UnityEngine;
using Ink.Runtime;

/// <summary>
/// DialogueTrigger is responsible for initializing and starting NPC dialogues.
/// Responsibilities:
/// - Loads the Ink story assigned to this NPC.
/// - Notifies the DialogueManager to start or continue conversations.
/// - Controls the dialogue options canvas visibility.
/// </summary>
[RequireComponent(typeof(NPCStateManager))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    private NPCStateManager _npcStateManager;   // Reference to the NPC's state manager

    [Header("UI")]
    [SerializeField] private GameObject _optionsCanvas; // Canvas with dialogue choices

    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJSON; // Ink dialogue file assigned to this NPC

    [Header("Dialogue State")]
    private Story _myStory;      // Local Ink story instance for this NPC
    private bool _hasTriggered;  // Prevents restarting the conversation multiple times

    #region Unity Methods
    private void Awake()
    {
        _npcStateManager = GetComponent<NPCStateManager>();

        if (_optionsCanvas != null)
            _optionsCanvas.SetActive(false);
    }

    private void Start()
    {
        _hasTriggered = false;
    }

    private void Update()
    {
        // Dialogue should only show if the NPC allows it and no other dialogue is playing
        if (_npcStateManager.CanShowCanvas && !DialogueManager.Instance.DialogueIsPlaying)
        {
            EnsureStoryLoaded();

            // Assign this NPC's story to the DialogueManager
            DialogueManager.Instance.CurrentStory = _myStory;

            // Start the conversation only once
            if (!_hasTriggered)
            {
                DialogueManager.Instance.StartConversation();
                _hasTriggered = true;
            }

            DialogueManager.Instance.DisplayChoices();

            if (_optionsCanvas != null)
                _optionsCanvas.SetActive(true);
        }
        else
        {
            if (_optionsCanvas != null)
                _optionsCanvas.SetActive(false);
        }
    }
    #endregion

    #region Dialogue
    /// <summary>
    /// Ensures that the Ink story is initialized for this NPC.
    /// </summary>
    private void EnsureStoryLoaded()
    {
        if (_myStory == null && _inkJSON != null)
            _myStory = new Story(_inkJSON.text);
    }

    /// <summary>
    /// Manually triggers dialogue mode (used when selecting dialogue options).
    /// </summary>
    public void Talk()
    {
        DialogueManager.Instance.EnterDialogueMode();
    }
    #endregion
}
