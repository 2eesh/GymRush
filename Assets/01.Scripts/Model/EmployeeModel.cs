using UnityEngine;

public class EmployeeModel : CharacterModel
{
    public EmployeeModel(float moveSpeed, float guideGaugeRatePerSecond)
        : base(Vector2.zero, moveSpeed, guideGaugeRatePerSecond)
    {
        Setup();
    }

    public void Setup()
    {
        Direction = Vector2.zero;
    }
}
