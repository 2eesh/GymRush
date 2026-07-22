using System.Collections.Generic;
using System.Linq;

public class EmployeeStateMachine : StateMachine<EmployeeState>
{
    private readonly Dictionary<EmployeeState, IState<EmployeeState>> _states;

    public EmployeeStateMachine(EmployeePresenter owner)
    {
        IState<EmployeeState>[] states =
        {
            new EmployeeIdleState(owner),
            new EmployeeMoveToWorkState(owner),
            new EmployeeWorkState(owner),
        };

        _states = states.ToDictionary(s => s.StateId);
    }

    public void ChangeState(EmployeeState state) => ChangeState(_states[state]);
}
