using UnityEngine;

[RequireComponent(typeof(PlayerView), typeof(PlayerInputController))]
public class PlayerInstaller : MonoBehaviour
{
    [SerializeField] VirtualJoystick _joystick;
    
    private void Start()
    {
        PlayerView view = GetComponent<PlayerView>();
        PlayerInputController controller = GetComponent<PlayerInputController>();

        PlayerModel model = new PlayerModel(Vector2.down, 5.0f);
        PlayerPresenter presenter = new PlayerPresenter(model, view);
        
        view.Construct(presenter);
        controller.Construct(presenter, _joystick);
    }
}
