using System.Collections.Generic;
using System.Linq;

public class EquipmentStateMachine : StateMachine<EquipmentState>
{
    private readonly Dictionary<EquipmentState, IState<EquipmentState>> _states;

    public EquipmentStateMachine(EquipmentModel model, IEquipmentView view)
    {
        IState<EquipmentState>[] states =
        {
            new EquipmentEmptyState(model, view),
            new EquipmentUsingState(model, view),
            new EquipmentDirtyState(model, view),
        };

        _states = states.ToDictionary(s => s.StateId);
    }
    
    public void ChangeState(EquipmentState state) => ChangeState(_states[state]);
}
