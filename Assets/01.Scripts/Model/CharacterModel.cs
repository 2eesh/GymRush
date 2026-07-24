using UnityEngine;

public abstract class CharacterModel
{
    public Vector2 Direction { get; set; }

    public CharacterStat MoveSpeed { get; }
    public CharacterStat GuideGaugeRatePerSecond { get; }

    public CharacterModel(Vector2 direction, float moveSpeed, float guideGaugeRatePerSecond)
    {
        Direction = direction;
        MoveSpeed = new CharacterStat(moveSpeed);
        GuideGaugeRatePerSecond = new CharacterStat(guideGaugeRatePerSecond);
    }
}
