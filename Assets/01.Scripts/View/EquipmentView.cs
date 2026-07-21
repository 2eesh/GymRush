using UnityEngine;

public class EquipmentView : MonoBehaviour, IEquipmentView
{
    [SerializeField] private Transform _seatPoint;
    [SerializeField] private Transform _rendererRoot;
    [SerializeField] private GameObject _cleaningZone;

    [SerializeField] private Color _usingTint = new Color(1f, 0.95f, 0.6f);
    [SerializeField] private Color _dirtyTint = new Color(0.6f, 0.55f, 0.5f);

    private EquipmentPresenter _presenter;
    private SpriteRenderer[] _renderers;
    private Color[] _baseColors;
    private GuideInterationGaugeZoneController _cleaningZoneController;

    public EquipmentPresenter Presenter => _presenter;
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

        if (_cleaningZone != null)
        {
            _cleaningZoneController = _cleaningZone.GetComponentInChildren<GuideInterationGaugeZoneController>(true);
        }
    }

    private void OnEnable()
    {
        if (_cleaningZoneController != null)
        {
            _cleaningZoneController.OnGaugeComplete += HandleCleanComplete;
        }
    }

    private void OnDisable()
    {
        if (_cleaningZoneController != null)
        {
            _cleaningZoneController.OnGaugeComplete -= HandleCleanComplete;
        }
    }

    private void HandleCleanComplete()
    {
        _presenter.Clean();
    }

    private void Start()
    {
        _presenter.Setup();
    }

    public void Construct(EquipmentPresenter presenter)
    {
        _presenter = presenter;
    }

    public void UpdateState(EquipmentState state)
    {
        if (_cleaningZone != null)
        {
            _cleaningZone.SetActive(state == EquipmentState.Dirty);
        }

        Color tint = state switch
        {
            EquipmentState.Using => _usingTint,
            EquipmentState.Dirty => _dirtyTint,
            _ => Color.white,
        };

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].color = _baseColors[i] * tint;
        }
    }

    public void UpdateDurability(int current, int max)
    {
        // TODO: 내구도 게이지 UI 연결
    }

}
