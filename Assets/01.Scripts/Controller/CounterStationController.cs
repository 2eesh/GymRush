using System.Linq;
using UnityEngine;

// 슬롯 1개 + GuideMaker 게이지 완료 이벤트를 중계하는 카운터 전용 존
public class CounterStationController : StationControllerBase
{
    [SerializeField] private GameObject _serviceGaugeRoot;
    [SerializeField] private CounterView[] _counters;

    protected override IUsableSlot[] Slots => _counters.Select(locker => (IUsableSlot)locker.Presenter).ToArray();
    
    private GuideInterationGaugeZoneController _gaugeController;
    private GuestPresenter _servingGuest;

    private void Awake()
    {
        _gaugeController = _serviceGaugeRoot.GetComponentInChildren<GuideInterationGaugeZoneController>(true);
    }

    private void Start()
    {
        _serviceGaugeRoot.SetActive(false);
    }

    private void OnEnable()
    {
        _gaugeController.OnGaugeComplete += HandleGaugeComplete;
    }

    private void OnDisable()
    {
        _gaugeController.OnGaugeComplete -= HandleGaugeComplete;
    }

    private void HandleGaugeComplete()
    {
        GuestPresenter guest = _servingGuest;
        _servingGuest = null;

        _serviceGaugeRoot.SetActive(false);
        
        FinishService(guest);

        guest.NotifyServiceComplete();
    }

    public override void BeginService(GuestPresenter guest)
    {
        _servingGuest = guest;
        _serviceGaugeRoot.SetActive(true);
    }
}
