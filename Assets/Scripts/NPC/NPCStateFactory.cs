using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NPCStates
{
    sad,
    neutral
}

public class NPCStateFactory
{
    NPCStateManager _context;
    Dictionary<NPCStates, NPCBaseState> _states = new Dictionary<NPCStates, NPCBaseState>();
    public NPCStateFactory(NPCStateManager currentContext)
    {
        _context = currentContext;
        _states[NPCStates.sad] = new NPCSadState(_context, this);
        _states[NPCStates.neutral] = new NPCNeutralState(_context, this);
    }
    public NPCBaseState Sad()
    {
        return _states[NPCStates.sad];
    }
    public NPCBaseState Neutral()
    {
        return _states[NPCStates.neutral];
    }
}
