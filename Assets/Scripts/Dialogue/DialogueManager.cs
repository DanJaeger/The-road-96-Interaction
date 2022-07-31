using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Ink.UnityIntegration;

public class DialogueManager : MonoBehaviour
{
    NPCStateManager _currentNPC = null;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _subtitlesCanvas;
    [SerializeField] private TextMeshProUGUI _subtitlesText;
    [SerializeField] private TextMeshProUGUI _nameText;

    [Header("Globals Ink File")]
    [SerializeField] InkFile _globalsInkFile;

    [Header("Choices UI")]
    private TextMeshProUGUI[] _choicesText;
    int _choice1Index;
    int _choice2Index;

    [Header("Dialogue Variables")]
    DialogueVariables _dialogueVariables;
    const string c_talkAnimationValue = "talkAnimationValue";
    const string c_idleAnimationValue = "idleAnimationValue";
    const string c_audioValue = "audioValue";

    private Story _currentStory;
    bool _dialogueIsPlaying;

    Coroutine _currentCoroutine = null;

    private static DialogueManager _instance;
    public static DialogueManager Instance { get => _instance; }
    public NPCStateManager CurrentNPC { get => _currentNPC; set => _currentNPC = value; }
    public Story CurrentStory { get => _currentStory; set => _currentStory = value; }
    public int Choice1Index { get => _choice1Index; private set => _choice1Index = value; }
    public int Choice2Index { get => _choice2Index; private set => _choice2Index = value; }
    public bool DialogueIsPlaying { get => _dialogueIsPlaying; set => _dialogueIsPlaying = value; }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        _instance = this;

        _dialogueVariables = new DialogueVariables(_globalsInkFile.filePath);
    }
    private void Start()
    {
        _dialogueIsPlaying = false;
        _subtitlesCanvas.SetActive(false);
    }
    public void StartConversation()
    {
        _dialogueVariables.StartListening(_currentStory);
        _currentStory.Continue();
        if (!String.IsNullOrEmpty(_currentStory.currentText))
        {
            _nameText.text = _currentNPC.Npc.name + ": ";
            _dialogueIsPlaying = true;
            _subtitlesCanvas.SetActive(true);
            PlayAnimation();
            _subtitlesText.text = _currentStory.currentText;
            SetAudio();
        }
        else
        {
            _currentStory.ContinueMaximally();
        }
        ChangeOptions();
    }
    public void EnterDialogueMode()
    {
        _dialogueVariables.StartListening(_currentStory);

        _nameText.text = _currentNPC.Npc.name + ": ";
        _dialogueIsPlaying = true;
        _subtitlesCanvas.SetActive(true);
        ContinueStory();
    }
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
            Debug.LogWarning("No Continua");
            _currentNPC.CanInteract = false;
        }
        ChangeOptions();
    }
    void PlayAnimation()
    {
        var animationValue = (int)_currentStory.variablesState[c_talkAnimationValue];
        if (animationValue < _currentNPC.Npc.ChatAnimations.Length)
        {
            string currentAnimation = _currentNPC.Npc.ChatAnimations[animationValue];
            _currentNPC.Animator.Play(currentAnimation);
        }
    }
    void SetAudio()
    {
        var audioValue = (int)_currentStory.variablesState[c_audioValue];
        _currentNPC.PlayAudio(audioValue);
    }
    public void SetIdleAnimation()
    {
        if (_currentStory.variablesState[c_idleAnimationValue] != null)
        {
            var animationValue = (int)_currentStory.variablesState[c_idleAnimationValue];
            _currentNPC.Animator.SetFloat("AnimationValue", animationValue);
        }
    }
    public void ChangeOptions()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(WaitForNPCToFinishTalking(_currentNPC.AudioSource.clip.length + 0.5f));
    }
    IEnumerator WaitForNPCToFinishTalking(float timeToFinishTalking)
    {
        DisplayChoices();

        yield return new WaitForSeconds(timeToFinishTalking);

        _dialogueVariables.StopListening(_currentStory);
        _subtitlesCanvas.SetActive(false);
        _dialogueIsPlaying = false;
        _subtitlesText.text = "";
    }
    public void DisplayChoices()
    {
        if (_currentNPC != null)
            _choicesText = _currentNPC.GetComponentsInChildren<TextMeshProUGUI>();
        else
            _choicesText = FindObjectOfType<NPCStateManager>().GetComponentsInChildren<TextMeshProUGUI>();
        
        List<Choice> currentChoices = _currentStory.currentChoices;

        int index = 0;
        if (currentChoices.Count > 0)
        {
            _choice1Index = currentChoices[0].index;
            _choice2Index = currentChoices[1].index;
            foreach (Choice choice in currentChoices)
            {
                if (_choicesText.Length > 1)
                {
                    _choicesText[index].text = choice.text;
                    index++;
                }
            }
        }
        else
        {
            _currentNPC.CanInteract = false;
        }

    }
    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
