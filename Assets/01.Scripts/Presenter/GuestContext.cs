using System;
using UnityEngine;

public class GuestContext
{
    public Transform InsidePoint { get; }
    public Transform ExitPoint { get; }
    public IStation CounterStation { get; }
    public IStation LockerStation { get; }
    public IStation[] EquipmentStations { get; }
    
    public GuestContext(Transform insidePoint, Transform exitPoint, IStation counterStation, IStation lockerStation, IStation[] equipmentStations)
    {
        InsidePoint = insidePoint;
        ExitPoint = exitPoint;
        CounterStation = counterStation;
        LockerStation = lockerStation;
        EquipmentStations = equipmentStations;
    }
}
