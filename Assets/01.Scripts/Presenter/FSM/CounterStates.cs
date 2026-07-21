using UnityEngine;

public class CounterEmptyState : IState<CounterState>
{
    public CounterState StateId => CounterState.Empty;

    private readonly CounterModel _model;
    private readonly ICounterView _view;

    public CounterEmptyState(CounterModel model, ICounterView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(CounterState.Empty);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}

public class CounterUsingState : IState<CounterState>
{
    public CounterState StateId => CounterState.Using;

    private readonly CounterModel _model;
    private readonly ICounterView _view;

    public CounterUsingState(CounterModel model, ICounterView view)
    {
        _model = model;
        _view = view;
    }

    public void Enter()
    {
        _view.UpdateState(CounterState.Using);
    }

    public void Tick(float deltaTime) { }
    public void Exit() { }
}
