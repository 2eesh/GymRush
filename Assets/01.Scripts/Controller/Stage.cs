using UnityEngine;

public class Stage : SingletonMonoBehaviour<Stage>
{
    [SerializeField] private Transform _insidePoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private StationController _counterStation;
    [SerializeField] private StationController _lockerStation;
    [SerializeField] private StationController[] _equipmentStations;

    public GuestContext Context { get; private set; }

    public StationController CounterStation => _counterStation;
    public StationController[] EquipmentStations => _equipmentStations;

    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Setup()
    {
        IStation[] equipmentStations = new IStation[_equipmentStations.Length];
        for (int i = 0; i < _equipmentStations.Length; i++)
        {
            equipmentStations[i] = _equipmentStations[i];
        }

        Context = new GuestContext(_insidePoint, _exitPoint, _counterStation, _lockerStation, equipmentStations);
    }
}
