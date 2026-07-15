using UnityEngine;

[RequireComponent(typeof(PlayerView), typeof(PlayerInputController))]
public class PlayerInstaller : MonoBehaviour
{
    [SerializeField] VirtualJoystick _joystick;
    [SerializeField] private UIPlayerDataView _dataView;
    [SerializeField] private int _startMoney = 0;

    private void Start()
    {
        PlayerView view = GetComponent<PlayerView>();
        PlayerInputController controller = GetComponent<PlayerInputController>();

        PlayerModel model = new PlayerModel(Vector2.down, 5.0f, _startMoney);
        PlayerPresenter presenter = new PlayerPresenter(model, view);
        PlayerDataPresenter dataPresenter = new PlayerDataPresenter(model, _dataView);

        view.Construct(presenter);
        controller.Construct(presenter, _joystick);
    }
}
