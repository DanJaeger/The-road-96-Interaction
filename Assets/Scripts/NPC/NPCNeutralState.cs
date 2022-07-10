using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNeutralState : NPCBaseState
{
    const string c_talkAnimation = "Talk_2";
    public NPCNeutralState(NPCStateManager currentContext, NPCStateFactory npcStateFactory)
  : base(currentContext, npcStateFactory) { }

    public override void CheckSwitchStates()
    {
        
    }
    public override void StartConversation(int buttonNumber)
    {
        Context.Animator.Play(c_talkAnimation);
        Context.PlayAudio(2);
        Context.IsTalking = true;

        Context.SubtitlesUI.SetActive(true);
        Context.NpcName.text = Context.Npc.NPCName;
        Context.NpcDialogue.text = Context.Npc.Dialogue[2];

        Context.ChangeOptions(0);   
    }
    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsSadAnimationHash, false);
    }

    public override void UpdateState()
    {
        
    }
}
