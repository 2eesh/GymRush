using UnityEngine;

public interface IEmployeeView : ICharacterView
{
    Vector2 Position { get; }
    void SetPosition(Vector2 position);
}
