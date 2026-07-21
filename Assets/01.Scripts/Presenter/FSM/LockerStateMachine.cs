using System.Collections.Generic;
using System.Linq;

public class LockerStateMachine : StateMachine<LockerState>
{
    private readonly Dictionary<LockerState, IState<LockerState>> _states;

    public LockerStateMachine(LockerModel model, ILockerView view)
    {
        IState<LockerState>[] states =
        {
            new LockerEmptyState(model, view),
            new LockerUsingState(model, view),
        };

        _states = states.ToDictionary(s => s.StateId);
    }

    public void ChangeState(LockerState state) => ChangeState(_states[state]);
}
