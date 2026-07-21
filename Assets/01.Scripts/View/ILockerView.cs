using UnityEngine;

public interface ILockerView
{
    Transform SeatPoint { get; }
    void UpdateState(LockerState state);
}
