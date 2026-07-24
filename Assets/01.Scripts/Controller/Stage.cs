using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private int _stageId = 1;
    [SerializeField] private Transform _insidePoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private StationController _counterStation;
    [SerializeField] private StationController _lockerStation;
    [SerializeField] private StationController[] _equipmentStations;

    public int StageId => _stageId;
    public GuestContext Context { get; private set; }

    public StationController CounterStation => _counterStation;
    public StationController[] EquipmentStations => _equipmentStations;

    private void Awake()
    {
        Setup();
        StageManager.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if (StageManager.HasInstance)
        {
            StageManager.Instance.Unregister(this);
        }
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
