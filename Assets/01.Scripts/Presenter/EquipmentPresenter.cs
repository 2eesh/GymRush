using UnityEngine;

public class EquipmentPresenter
{
    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;
    private readonly StateMachine _fsm = new StateMachine();

    private readonly EquipmentEmptyState _emptyState;
    private readonly EquipmentUsingState _usingState;
    private readonly EquipmentDirtyState _dirtyState;

    public bool CanUse => _model.State == EquipmentState.Empty;
    public Transform SeatPoint => _view.SeatPoint;

    public EquipmentPresenter(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;

        _emptyState = new EquipmentEmptyState(model, view);
        _usingState = new EquipmentUsingState(model, view);
        _dirtyState = new EquipmentDirtyState(model, view);

        _fsm.ChangeState(_emptyState);
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);
    }

    public void StartUse()
    {
        if (_model.State != EquipmentState.Empty)
        {
            return;
        }

        _fsm.ChangeState(_usingState);
    }

    public void FinishUse()
    {
        if (_model.State != EquipmentState.Using)
        {
            return;
        }

        _model.ConsumeDurability();
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);

        if (_model.IsBroken)
        {
            _fsm.ChangeState(_dirtyState);
        }
        else
        {
            _fsm.ChangeState(_emptyState);
        }
    }

    public void Clean()
    {
        if (_model.State != EquipmentState.Dirty)
        {
            return;
        }

        _model.RepairFull();
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);
        _fsm.ChangeState(_emptyState);
    }
}
