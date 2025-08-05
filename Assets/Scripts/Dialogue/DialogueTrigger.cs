using UnityEngine;
using Ink.Runtime;

[RequireComponent(typeof(NPCStateManager))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    NPCStateManager _npcStateManager;

    [Header("Canvas")]
    [SerializeField] GameObject _optionsCanvas;

    [Header("Ink JSON")]
    [SerializeField] TextAsset _inkJSON;

    [Header("Display Dialogue")]
    Story _myStory;
    bool _isTrigger;

    private void Awake()
    {
        _npcStateManager = GetComponent<NPCStateManager>();
        _optionsCanvas.SetActive(false);
    }
    private void Start()
    {
        _isTrigger = false;
    }

     private void Update()
     {
         if (_npcStateManager.CanShowCanvas && !DialogueManager.Instance.DialogueIsPlaying)
         {
             if (_myStory == null)
             {
                 _myStory = new Story(_inkJSON.text);
             }
             DialogueManager.Instance.CurrentStory = _myStory;
             if (!_isTrigger)
             {
                 DialogueManager.Instance.StartConversation();
                 _isTrigger = true;
             }
             DialogueManager.Instance.DisplayChoices();
             _optionsCanvas.SetActive(true);
         }
         else
         {
             _optionsCanvas.SetActive(false);
         }
     }
    public void Talk()
    {
        DialogueManager.Instance.EnterDialogueMode();
    }
}
