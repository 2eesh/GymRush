using UnityEngine;

public class LockerPresenter : IUsableSlot
{
    private readonly LockerModel _model;
    private readonly ILockerView _view;
    private readonly LockerStateMachine _fsm;

    public bool CanUse => _fsm.CurrentState.StateId == LockerState.Empty;
    public Transform SeatPoint => _view.SeatPoint;

    public LockerPresenter(LockerModel model, ILockerView view)
    {
        _model = model;
        _view = view;
        _fsm = new LockerStateMachine(model, view);
    }

    public void Setup()
    {
        _fsm.ChangeState(LockerState.Empty);
    }

    public void StartUse()
    {
        if (_fsm.CurrentState.StateId != LockerState.Empty)
        {
            return;
        }

        _fsm.ChangeState(LockerState.Using);
    }

    public void FinishUse()
    {
        if (_fsm.CurrentState.StateId != LockerState.Using)
        {
            return;
        }

        _fsm.ChangeState(LockerState.Empty);
    }
}
