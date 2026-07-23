using System;
using UnityEngine;

public interface IGuestView : ICharacterView
{
    Vector2 Position { get; }
    void SetPosition(Vector2 position);
    void ThrowMoney(Vector3 target, Action onArrived);
}
