using UnityEngine;

public class NPCStateManager : MonoBehaviour
{
    [Header(header: "Components")]
    Animator _animator;
    AudioSource _audioSource;

    [Header(header: "NPC Settings")]
    [SerializeField] NPC _npc;

    [Header(header: "States")]
    bool _canInteract;
    bool _canShowCanvas;
    int _isSadAnimationHash;

    public Animator Animator { get => _animator;}
    public NPC Npc { get => _npc; set => _npc = value; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public int IsSadAnimationHash { get => _isSadAnimationHash;}
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public bool CanShowCanvas { get => _canShowCanvas; set => _canShowCanvas = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }
    private void Start()
    {
        _isSadAnimationHash = Animator.StringToHash("IsSad");

        _canInteract = true;
        _canShowCanvas = false;
    }
    public void PlayAudio(int clipNumber)
    {
        _audioSource.clip = _npc.ChatAudioClips[clipNumber];
        _audioSource.Play();
    }
    public void SetAnimation()
    {
        DialogueManager.Instance.SetIdleAnimation();
    }
}
