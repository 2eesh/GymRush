using UnityEngine;

public class JoystickMoveInputSource : IMoveInputSource
{
    private readonly VirtualJoystick _joystick;
    public JoystickMoveInputSource(VirtualJoystick joystick)
    {
        _joystick = joystick;
    }

    public Vector2 GetDirection()
    {
        return _joystick.Direction;
    }
}
