using UnityEngine;

public class CounterPresenter : IUsableSlot
{
    private readonly CounterModel _model;
    private readonly ICounterView _view;
    private readonly CounterStateMachine _fsm;

    public bool CanUse => _fsm.CurrentState.StateId == CounterState.Empty;
    public Transform SeatPoint => _view.SeatPoint;

    public CounterPresenter(CounterModel model, ICounterView view)
    {
        _model = model;
        _view = view;
        _fsm = new CounterStateMachine(model, view);
    }

    public void Setup()
    {
        _fsm.ChangeState(CounterState.Empty);
    }

    public void StartUse()
    {
        if (_fsm.CurrentState.StateId != CounterState.Empty)
        {
            return;
        }

        _fsm.ChangeState(CounterState.Using);
    }

    public void FinishUse()
    {
        if (_fsm.CurrentState.StateId != CounterState.Using)
        {
            return;
        }

        _fsm.ChangeState(CounterState.Empty);
    }
}
