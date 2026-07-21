using UnityEngine;

public interface IGuestView : ICharacterView
{
    Vector2 Position { get; }
    void SetPosition(Vector2 position);
    void DropMoney(Vector2 position, int amount);
}
