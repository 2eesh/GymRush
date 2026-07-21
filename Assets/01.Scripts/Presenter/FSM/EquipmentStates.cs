public class EquipmentEmptyState : IState<EquipmentState>
{
    public EquipmentState StateId => EquipmentState.Empty;

    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentEmptyState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(EquipmentState.Empty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class EquipmentUsingState : IState<EquipmentState>
{
    public EquipmentState StateId => EquipmentState.Using;

    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentUsingState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(EquipmentState.Using);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class EquipmentDirtyState : IState<EquipmentState>
{
    public EquipmentState StateId => EquipmentState.Dirty;

    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentDirtyState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(EquipmentState.Dirty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}
