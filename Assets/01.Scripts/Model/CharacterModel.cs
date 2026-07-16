using UnityEngine;

public abstract class CharacterModel
{
    public Vector2 Direction { get; set; }
    
    // TODO: stat 클래스 만들어서 분리
    public float MoveSpeed { get; set; }
    public float GuideGaugeRatePerSecond { get; set; }
    
    public CharacterModel(Vector2 direction, float moveSpeed, float guideGaugeRatePerSecond)
    {
        Direction = direction;
        MoveSpeed = moveSpeed;
        GuideGaugeRatePerSecond = guideGaugeRatePerSecond;
    }
}
