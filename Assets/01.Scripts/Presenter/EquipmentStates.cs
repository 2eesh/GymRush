public class EquipmentEmptyState : IState
{
    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentEmptyState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _model.State = EquipmentState.Empty;
        _view.UpdateState(EquipmentState.Empty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class EquipmentUsingState : IState
{
    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentUsingState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _model.State = EquipmentState.Using;
        _view.UpdateState(EquipmentState.Using);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class EquipmentDirtyState : IState
{
    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;

    public EquipmentDirtyState(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _model.State = EquipmentState.Dirty;
        _view.UpdateState(EquipmentState.Dirty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}
