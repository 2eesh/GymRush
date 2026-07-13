using UnityEngine;

public class CharacterModel
{
    public Vector2 Direction { get; set; }
    public float MoveSpeed { get; set; }
    
    public CharacterModel(Vector2 direction, float moveSpeed)
    {
        Direction = direction;
        MoveSpeed = moveSpeed;
    }
}
