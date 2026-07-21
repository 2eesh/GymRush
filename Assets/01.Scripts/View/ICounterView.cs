using UnityEngine;

public interface ICounterView
{
    Transform SeatPoint { get; }
    void UpdateState(CounterState state);
}
