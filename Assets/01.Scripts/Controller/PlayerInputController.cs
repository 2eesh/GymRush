using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerPresenter _presenter;
    private IMoveInputSource _keyboardSource;
    private IMoveInputSource _joystickSource;

    public void Construct(PlayerPresenter presenter, VirtualJoystick joystick)
    {
        _presenter = presenter;
        _keyboardSource = new KeyboardMoveInputSource();
        _joystickSource = new JoystickMoveInputSource(joystick);
    }

    private void Update()
    {
        Vector2 direction = Vector2.ClampMagnitude(_keyboardSource.GetDirection() + _joystickSource.GetDirection(), 1.0f);
        _presenter.SetDirection(direction);
    }
}
