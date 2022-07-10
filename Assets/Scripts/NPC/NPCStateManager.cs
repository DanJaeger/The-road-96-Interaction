using System.Collections;
using UnityEngine;
using TMPro;

public class NPCStateManager : MonoBehaviour
{
    NPCBaseState _currentState;
    NPCStateFactory _states;
    Animator _animator;
    AudioSource _audioSource;

    [SerializeField] NPC _npc;
    [SerializeField] GameObject _canvas;

    [SerializeField] GameObject _player;
    [SerializeField] GameObject _subtitlesUI;

    [SerializeField] TextMeshProUGUI _npcName;
    [SerializeField] TextMeshProUGUI _npcDialogue;

    [SerializeField] TextMeshProUGUI _option1;
    [SerializeField] TextMeshProUGUI _option2;

    Coroutine _currentCoroutine = null;

    bool _canInteract;
    bool _isTalking;
    int _isSadAnimationHash;
    public NPCBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Animator Animator { get => _animator;}
    public NPC Npc { get => _npc; set => _npc = value; }
    public GameObject SubtitlesUI { get => _subtitlesUI; set => _subtitlesUI = value; }
    public TextMeshProUGUI Option1 { get => _option1; set => _option1 = value; }
    public TextMeshProUGUI Option2 { get => _option2; set => _option2 = value; }
    public TextMeshProUGUI NpcDialogue { get => _npcDialogue; set => _npcDialogue = value; }
    public TextMeshProUGUI NpcName { get => _npcName; set => _npcName = value; }
    public bool IsTalking { get => _isTalking; set => _isTalking = value; }
    public bool CanInteract { get => _canInteract;}
    public int IsSadAnimationHash { get => _isSadAnimationHash;}
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }
    private void Start()
    {
        _isSadAnimationHash = Animator.StringToHash("IsSad");
        
        _subtitlesUI.SetActive(false);

        _canInteract = true;
        _isTalking = false;

        _states = new NPCStateFactory(this);
        _currentState = _states.Sad();
        _currentState.EnterState();
    }
    private void Update()
    {
        _currentState.UpdateState();
    }

    public void DisplayCanvas()
    {
        _canvas.SetActive(true);
    }
    public void HideCanvas()
    {
        _canvas.SetActive(false);
    }
    public void Talk(int buttonNumber)
    {
        _currentState.StartConversation(buttonNumber);
    }
    
    public void ChangeOptions(int buttonNumber)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(WaitForNPCToFinishTalking(buttonNumber));
    }

    IEnumerator WaitForNPCToFinishTalking(int buttonNumber)
    {
        if (_currentState != _states.Neutral())
        {
            HideCanvas();
            yield return new WaitForSeconds(2.5f);
            _option1.text = _npc.ChatOptions[buttonNumber];
            _option2.text = _npc.ChatOptions[buttonNumber + 1];
            EndDialogue();
            DisplayCanvas();
        }
        else
        {
            HideCanvas();
            yield return new WaitForSeconds(4.0f);
            EndDialogue();
            _canInteract = false;
        }
    }
    void EndDialogue()
    {
        _isTalking = false;
        _subtitlesUI.SetActive(false);
    }
    public void PlayAudio(int clipNumber)
    {
        _audioSource.clip = _npc.ChatAudioClips[clipNumber];
        _audioSource.Play();
    }
}
