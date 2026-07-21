using System.Collections.Generic;
using System.Linq;

public class GuestStateMachine : StateMachine<GuestState>
{
    private readonly Dictionary<GuestState, IState<GuestState>> _states;

    public GuestStateMachine(GuestPresenter owner)
    {
        IState<GuestState>[] states =
        {
            new GuestEnterState(owner),
            new GuestDecideNextState(owner),
            new GuestMoveToZoneState(owner),
            new GuestWaitInQueueState(owner),
            new GuestUseStationState(owner),
            new GuestDropMoneyState(owner),
            new GuestExitState(owner),
        };

        _states = states.ToDictionary(s => s.StateId);
    }

    public void ChangeState(GuestState state) => ChangeState(_states[state]);
}
