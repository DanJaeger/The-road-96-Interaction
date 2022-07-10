using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSadState : NPCBaseState
{
    bool _canChangeToNeutral;
    int _phase;

    const string c_talkAnimation = "Talk_1";
    public NPCSadState(NPCStateManager currentContext, NPCStateFactory npcStateFactory)
   : base(currentContext, npcStateFactory) { }
    public override void CheckSwitchStates()
    {
        if (_canChangeToNeutral)
        {
            SwitchState(Factory.Neutral());
        }
    }

    public override void EnterState()
    {
        InitState();
    }

    void InitState()
    {
        Context.Animator.SetBool(Context.IsSadAnimationHash, true);

        Context.Option1.text = Context.Npc.ChatOptions[0];
        Context.Option2.text = Context.Npc.ChatOptions[1];

        _phase = 0;
    }

    public override void StartConversation(int buttonNumber)
    {
        Context.IsTalking = true;
        SetSubtitles();

        if (buttonNumber == 0)
        {
            if (_phase == 0)
            {
                TalkWithoutAnimation();
            }
            else
            {
                TalkWithAnimation();
            }
        }
        else
        {
            TalkWithAnimation();
        }
    }

    void SetSubtitles()
    {
        Context.SubtitlesUI.SetActive(true);
        Context.NpcName.text = Context.Npc.NPCName;
    }

    void TalkWithAnimation()
    {
        Context.Animator.Play(c_talkAnimation);
        Context.PlayAudio(1);
        Context.NpcDialogue.text = Context.Npc.Dialogue[1];
        Context.ChangeOptions(4);
        _canChangeToNeutral = true;
    }
    void TalkWithoutAnimation()
    {
        Context.PlayAudio(0);
        Context.NpcDialogue.text = Context.Npc.Dialogue[0];
        Context.ChangeOptions(2);
        _phase++;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
