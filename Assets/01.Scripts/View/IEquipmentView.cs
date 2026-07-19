using UnityEngine;

public interface IEquipmentView
{
    Transform SeatPoint { get; }
    void UpdateState(EquipmentState state);
    void UpdateDurability(int current, int max);
}
