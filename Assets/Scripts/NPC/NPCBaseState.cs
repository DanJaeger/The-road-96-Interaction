public abstract class NPCBaseState
{
    NPCStateManager _context;
    NPCStateFactory _factory;

    public NPCStateManager Context { get => _context; set => _context = value; }
    public NPCStateFactory Factory { get => _factory; set => _factory = value; }

    public NPCBaseState(NPCStateManager currentContext, NPCStateFactory playerStateFactory)
    {
        _context = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void CheckSwitchStates();
    public abstract void StartConversation(int buttonNumber);

    protected void SwitchState(NPCBaseState newState)
    {
        Context.CurrentState = newState;
        newState.EnterState();
    }
}
