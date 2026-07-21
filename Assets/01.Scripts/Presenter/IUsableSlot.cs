using UnityEngine;

public interface IUsableSlot
{
    bool CanUse { get; }
    Transform SeatPoint { get; }
    void StartUse();
    void FinishUse();
}
