using UnityEngine;

public interface ICharacterView
{
    void SetVelocity(Vector2 velocity);
    float GuideGaugeRatePerSecond { get; set; }
}
