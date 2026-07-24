using System;
using UnityEngine;

// 게이지 서비스 유닛의 부속물 — 자기 서비스 게이지 존과 직원 근무점을 소유한다.
// ContentUnit과 같은 오브젝트(Content_N 루트)에 부착.
public class GaugeServicePoint : MonoBehaviour
{
    [SerializeField] private GameObject _serviceGaugeRoot;

    // 담당 직원이 상주할 위치 — 서비스 게이지 존 콜라이더 안쪽에 배치할 것
    [SerializeField] private Transform _employeeWorkPoint;

    public Transform EmployeeWorkPoint => _employeeWorkPoint;

    public event Action OnServiceComplete;

    private GuideInterationGaugeZoneController _gaugeController;

    private void Awake()
    {
        _gaugeController = _serviceGaugeRoot.GetComponentInChildren<GuideInterationGaugeZoneController>(true);

        if (_gaugeController == null)
        {
            Debug.LogWarning($"[{name}] ServiceGaugeRoot에서 게이지 존을 찾지 못했습니다.", this);
        }
    }

    private void Start()
    {
        HideGauge();
    }

    private void OnEnable()
    {
        if (_gaugeController != null)
        {
            _gaugeController.OnGaugeComplete += HandleGaugeComplete;
        }
    }

    private void OnDisable()
    {
        if (_gaugeController != null)
        {
            _gaugeController.OnGaugeComplete -= HandleGaugeComplete;
        }
    }

    public void ShowGauge()
    {
        _serviceGaugeRoot.SetActive(true);
    }

    public void HideGauge()
    {
        _serviceGaugeRoot.SetActive(false);
    }

    private void HandleGaugeComplete()
    {
        OnServiceComplete?.Invoke();
    }
}
