using UnityEngine;

public class EmployeeModel : CharacterModel
{
    public string Id { get; }
    public EmployeeRole Role { get; }

    public EmployeeModel(string id, EmployeeRole role, float moveSpeed, float guideGaugeRatePerSecond)
        : base(Vector2.zero, moveSpeed, guideGaugeRatePerSecond)
    {
        Id = id;
        Role = role;
        Setup();
    }

    public void Setup()
    {
        Direction = Vector2.zero;
    }
}
