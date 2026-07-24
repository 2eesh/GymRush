using UnityEngine;

public class LockerView : MonoBehaviour, ILockerView, ISlotViewProvider
{
    [SerializeField] private Transform _seatPoint;
    [SerializeField] private Transform _rendererRoot;
    [SerializeField] private Color _usingTint = new Color(1f, 0.95f, 0.6f);

    private LockerPresenter _presenter;
    private SpriteRenderer[] _renderers;
    private Color[] _baseColors;

    public LockerPresenter Presenter => _presenter;
    public IUsableSlot Slot => _presenter;
    public Transform SeatPoint => _seatPoint;

    private void Awake()
    {
        if (_rendererRoot != null)
        {
            _renderers = _rendererRoot.GetComponentsInChildren<SpriteRenderer>(true);
        }
        else
        {
            _renderers = new SpriteRenderer[0];
        }

        _baseColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _baseColors[i] = _renderers[i].color;
        }
    }

    private void Start()
    {
        _presenter.Setup();
    }

    public void Construct(LockerPresenter presenter)
    {
        _presenter = presenter;
    }

    public void UpdateState(LockerState state)
    {
        Color tint = state == LockerState.Using ? _usingTint : Color.white;

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].color = _baseColors[i] * tint;
        }
    }
}
