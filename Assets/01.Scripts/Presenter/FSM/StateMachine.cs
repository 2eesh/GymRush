public class StateMachine<TStateId>
{
    public IState<TStateId> CurrentState { get; private set; }

    protected void ChangeState(IState<TStateId> next)
    {
        if (next == null || next == CurrentState)
        {
            return;
        }

        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void Tick(float deltaTime)
    {
        CurrentState?.Tick(deltaTime);
    }
}
