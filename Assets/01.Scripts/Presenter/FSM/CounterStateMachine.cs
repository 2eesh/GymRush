using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CounterStateMachine : StateMachine<CounterState>
{
    private readonly Dictionary<CounterState, IState<CounterState>> _states;

    public CounterStateMachine(CounterModel model, ICounterView view)
    {
        IState<CounterState>[] states =
        {
            new CounterEmptyState(model, view),
            new CounterUsingState(model, view),
        };

        _states = states.ToDictionary(s => s.StateId);
    }

    public void ChangeState(CounterState state) => ChangeState(_states[state]);
}
