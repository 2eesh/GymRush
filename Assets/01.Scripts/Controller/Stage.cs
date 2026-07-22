using System.Linq;
using UnityEngine;

public class Stage : SingletonMonoBehaviour<Stage>
{
    [SerializeField] private Transform _insidePoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private CounterStationController _counterStation;
    [SerializeField] private LockerStationController _lockerStation;
    [SerializeField] private EquipmentZoneStationController[] _equipmentStations;
    
    public GuestContext Context { get; private set; }

    public CounterStationController CounterStation => _counterStation;
    public EquipmentZoneStationController[] EquipmentStations => _equipmentStations;
    
    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Setup()
    {
        IStation[] equipmentStations = _equipmentStations.Cast<IStation>().ToArray();
        Context = new GuestContext(_insidePoint, _exitPoint, _counterStation, _lockerStation, equipmentStations);
    }
}
