public class LockerEmptyState : IState<LockerState>
{
    public LockerState StateId => LockerState.Empty;

    private readonly LockerModel _model;
    private readonly ILockerView _view;

    public LockerEmptyState(LockerModel model, ILockerView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(LockerState.Empty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class LockerUsingState : IState<LockerState>
{
    public LockerState StateId => LockerState.Using;

    private readonly LockerModel _model;
    private readonly ILockerView _view;

    public LockerUsingState(LockerModel model, ILockerView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(LockerState.Using);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}
