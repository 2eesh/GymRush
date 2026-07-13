using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMoveInputSource : IMoveInputSource
{
    public Vector2 GetDirection()
    {
        Vector2 direction = Vector2.zero;
        if (Keyboard.current.aKey.isPressed)
        {
            direction += Vector2.left;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            direction += Vector2.down;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            direction += Vector2.up;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            direction += Vector2.right;
        }

        return direction.normalized;
    }
}
