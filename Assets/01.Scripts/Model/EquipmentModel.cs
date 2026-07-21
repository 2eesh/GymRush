using UnityEngine;

public class EquipmentModel
{
    public int MaxDurability { get; }
    public int CurrentDurability { get; private set; }
    
    public bool IsBroken => CurrentDurability <= 0;

    public EquipmentModel(int maxDurability)
    {
        MaxDurability = Mathf.Max(1, maxDurability);
        CurrentDurability = MaxDurability;
    }

    public void ConsumeDurability()
    {
        CurrentDurability = Mathf.Max(0, CurrentDurability - 1);
    }

    public void RepairFull()
    {
        CurrentDurability = MaxDurability;
    }
}
