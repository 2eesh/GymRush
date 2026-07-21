using System.Linq;
using UnityEngine;

public class LockerStationController : StationControllerBase
{
    [SerializeField] private LockerView[] _lockers;

    protected override IUsableSlot[] Slots => _lockers.Select(locker => (IUsableSlot)locker.Presenter).ToArray();
}
