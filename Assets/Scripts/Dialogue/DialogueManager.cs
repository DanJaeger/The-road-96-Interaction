using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Ink.UnityIntegration;

/// <summary>
/// DialogueManager controls Ink dialogues flow.
/// Responsibilities:
/// - Displaying subtitles and choices.
/// - Controlling NPC animations during dialogue.
/// - Playing NPC voice/audio.
/// - Handling Ink story progression and choices.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _subtitlesCanvas;           // Dialogue canvas (subtitles panel)
    [SerializeField] private TextMeshProUGUI _subtitlesText;        // Current dialogue text
    [SerializeField] private TextMeshProUGUI _nameText;             // NPC name text

    [Header("Globals Ink File")]
    [SerializeField] private InkFile _globalsInkFile;               // Ink global variables file

    [Header("Choices UI")]
    private TextMeshProUGUI[] _choicesText;                         // Choices displayed on screen
    private int _choice1Index;
    private int _choice2Index;

    [Header("Ink Variables")]
    private DialogueVariables _dialogueVariables;
    private const string TALK_ANIM = "talkAnimationValue";
    private const string IDLE_ANIM = "idleAnimationValue";
    private const string AUDIO_VAL = "audioValue";

    [Header("Story Variables")]
    private Story _currentStory;                                    // Active Ink story
    private bool _dialogueIsPlaying;

    private Coroutine _talkingCoroutine = null;

    // Current NPC reference
    private NPCStateManager _currentNPC = null;

    // Singleton instance
    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;

    #region Properties
    public NPCStateManager CurrentNPC { get => _currentNPC; set => _currentNPC = value; }
    public Story CurrentStory { get => _currentStory; set => _currentStory = value; }
    public int Choice1Index => _choice1Index;
    public int Choice2Index => _choice2Index;
    public bool DialogueIsPlaying { get => _dialogueIsPlaying; set => _dialogueIsPlaying = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
        }
        _instance = this;

        _dialogueVariables = new DialogueVariables(_globalsInkFile.filePath);
    }

    private void Start()
    {
        _dialogueIsPlaying = false;
        _subtitlesCanvas.SetActive(false);
    }
    #endregion

    #region Dialogue Flow
    /// <summary>
    /// Starts a new conversation with the current NPC.
    /// </summary>
    public void StartConversation()
    {
        _dialogueVariables.StartListening(_currentStory);

        _currentStory.Continue(); // Go to the first block
        if (!string.IsNullOrEmpty(_currentStory.currentText))
        {
            _nameText.text = _currentNPC.Npc.name + ": ";
            _dialogueIsPlaying = true;
            _subtitlesCanvas.SetActive(true);

            PlayAnimation();
            _subtitlesText.text = _currentStory.currentText;
            SetAudio();
        }

        ChangeOptions();
    }

    /// <summary>
    /// Enters dialogue mode (when story already started).
    /// </summary>
    public void EnterDialogueMode()
    {
        _dialogueVariables.StartListening(_currentStory);

        _nameText.text = _currentNPC.Npc.name + ": ";
        _dialogueIsPlaying = true;
        _subtitlesCanvas.SetActive(true);

        ContinueStory();
    }

    /// <summary>
    /// Advances the current Ink story if possible.
    /// </summary>
    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            PlayAnimation();
            _subtitlesText.text = _currentStory.Continue();
            SetAudio();
        }
        else
        {
            Debug.Log("Story finished or cannot continue.");
            _currentNPC.CanInteract = false;
        }

        ChangeOptions();
    }
    #endregion

    #region Animations & Audio
    /// <summary>
    /// Plays NPC talking animation (based on Ink variable).
    /// </summary>
    private void PlayAnimation()
    {
        if (_currentStory.variablesState.GlobalVariableExistsWithName(TALK_ANIM))
        {
            var animationValue = (float)_currentStory.variablesState[TALK_ANIM];
            StartCoroutine(PlayAnimationWithTransition(animationValue));
        }
    }

    /// <summary>
    /// Smooth transition between animation blend values.
    /// </summary>
    private IEnumerator PlayAnimationWithTransition(float animationValue)
    {
        if (!_currentNPC.Animator) yield break;

        float blendTime = 0.3f;
        float timer = 0f;
        float startBlend = _currentNPC.Animator.GetFloat("AnimationValue");
        float targetBlend = animationValue;

        while (timer < blendTime)
        {
            timer += Time.deltaTime;
            float blendValue = Mathf.Lerp(startBlend, targetBlend, timer / blendTime);
            _currentNPC.Animator.SetFloat("AnimationValue", blendValue);
            yield return null;
        }

        _currentNPC.Animator.SetFloat("AnimationValue", targetBlend);
    }

    /// <summary>
    /// Plays NPC voice/audio (based on Ink variable).
    /// </summary>
    private void SetAudio()
    {
        if (_currentStory.variablesState.GlobalVariableExistsWithName(AUDIO_VAL))
        {
            int audioValue = (int)_currentStory.variablesState[AUDIO_VAL];
            _currentNPC.PlayAudio(audioValue);
        }
    }

    /// <summary>
    /// Switches NPC animation back to idle after dialogue ends.
    /// </summary>
    public void SetIdleAnimation()
    {
        if (_currentStory.variablesState.GlobalVariableExistsWithName(IDLE_ANIM))
        {
            var animationValue = (float)_currentStory.variablesState[IDLE_ANIM];
            StartCoroutine(PlayAnimationWithTransition(animationValue));
        }
    }
    #endregion

    #region Choices Handling
    /// <summary>
    /// Displays available choices on screen.
    /// </summary>
    public void DisplayChoices()
    {
        _choicesText = _currentNPC != null
            ? _currentNPC.GetComponentsInChildren<TextMeshProUGUI>()
            : FindObjectOfType<NPCStateManager>()?.GetComponentsInChildren<TextMeshProUGUI>();

        if (_choicesText == null) return;

        List<Choice> currentChoices = _currentStory.currentChoices;
        if (currentChoices.Count > 0)
        {
            _choice1Index = currentChoices[0].index;
            _choice2Index = currentChoices.Count > 1 ? currentChoices[1].index : -1;

            for (int i = 0; i < currentChoices.Count && i < _choicesText.Length; i++)
            {
                _choicesText[i].text = currentChoices[i].text;
            }
        }
        else
        {
            _currentNPC.CanInteract = false;
        }
    }

    /// <summary>
    /// Selects a dialogue choice.
    /// </summary>
    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
    }
    #endregion

    #region Timing & Conversation End
    /// <summary>
    /// Sets delay before showing choices or ending conversation.
    /// </summary>
    private void ChangeOptions()
    {
        if (_talkingCoroutine != null)
            StopCoroutine(_talkingCoroutine);

        float waitTime = _currentNPC.AudioSource?.clip != null
            ? _currentNPC.AudioSource.clip.length + 0.5f
            : 0.5f;

        _talkingCoroutine = StartCoroutine(WaitForNPCToFinishTalking(waitTime));
    }

    private IEnumerator WaitForNPCToFinishTalking(float timeToFinishTalking)
    {
        DisplayChoices();
        yield return new WaitForSeconds(timeToFinishTalking);
        FinishConversation();
    }

    /// <summary>
    /// Ends dialogue with the current NPC.
    /// </summary>
    private void FinishConversation()
    {
        _dialogueVariables.StopListening(_currentStory);
        _subtitlesCanvas.SetActive(false);
        _dialogueIsPlaying = false;
        _subtitlesText.text = "";
        _currentNPC.CanShowCanvas = false;

        SetIdleAnimation();
    }
    #endregion
}
