using System.Linq;
using UnityEngine;

public class EquipmentZoneStationController : StationControllerBase
{
    [SerializeField] private EquipmentView[] _equipments;

    protected override IUsableSlot[] Slots => _equipments.Select(equipment => (IUsableSlot)equipment.Presenter).ToArray();
}
