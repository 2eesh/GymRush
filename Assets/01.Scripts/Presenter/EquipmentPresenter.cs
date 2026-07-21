using UnityEngine;

public class EquipmentPresenter : IUsableSlot
{
    private readonly EquipmentModel _model;
    private readonly IEquipmentView _view;
    private readonly EquipmentStateMachine _fsm;

    public bool CanUse => _fsm.CurrentState.StateId == EquipmentState.Empty;
    public Transform SeatPoint => _view.SeatPoint;

    public EquipmentPresenter(EquipmentModel model, IEquipmentView view)
    {
        _model = model;
        _view = view;
        _fsm = new EquipmentStateMachine(model, view);
    }

    public void Setup()
    {
        _fsm.ChangeState(EquipmentState.Empty);
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);
    }

    public void StartUse()
    {
        if (_fsm.CurrentState.StateId != EquipmentState.Empty)
        {
            return;
        }

        _fsm.ChangeState(EquipmentState.Using);
    }

    public void FinishUse()
    {
        if (_fsm.CurrentState.StateId != EquipmentState.Using)
        {
            return;
        }

        _model.ConsumeDurability();
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);

        _fsm.ChangeState(_model.IsBroken ? EquipmentState.Dirty : EquipmentState.Empty);
    }

    public void Clean()
    {
        if (_fsm.CurrentState.StateId != EquipmentState.Dirty)
        {
            return;
        }

        _model.RepairFull();
        _view.UpdateDurability(_model.CurrentDurability, _model.MaxDurability);
        _fsm.ChangeState(EquipmentState.Empty);
    }
}
