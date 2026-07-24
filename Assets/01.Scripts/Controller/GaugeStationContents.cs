using System;
using System.Collections.Generic;
using UnityEngine;

// 직원이 게이지를 채워야 서비스가 끝나는 콘텐츠 — 카운터가 사용.
// 게이지·근무점은 각 유닛의 GaugeServicePoint가 소유하고, 여기서는 유닛별 진행만 중계한다.
public class GaugeStationContents : StationContents
{
    private readonly Dictionary<ContentUnit, GaugeServicePoint> _pointByUnit = new Dictionary<ContentUnit, GaugeServicePoint>();
    private readonly Dictionary<GaugeServicePoint, Action> _pendingByPoint = new Dictionary<GaugeServicePoint, Action>();
    private readonly Dictionary<GaugeServicePoint, Action> _handlers = new Dictionary<GaugeServicePoint, Action>();

    private GaugeServicePoint[] _points;
    private int _claimedWorkPointCount;

    protected override void Awake()
    {
        base.Awake();

        _points = GetComponentsInChildren<GaugeServicePoint>(true);

        foreach (GaugeServicePoint point in _points)
        {
            ContentUnit unit = point.GetComponent<ContentUnit>();
            if (unit == null)
            {
                Debug.LogWarning($"[{name}] GaugeServicePoint '{point.name}'와 같은 오브젝트에 ContentUnit이 없습니다.", point);
                continue;
            }

            _pointByUnit[unit] = point;
        }
    }

    private void OnEnable()
    {
        foreach (GaugeServicePoint point in _points)
        {
            GaugeServicePoint captured = point;
            Action handler = () => HandleServiceComplete(captured);
            _handlers[point] = handler;
            point.OnServiceComplete += handler;
        }
    }

    private void OnDisable()
    {
        foreach (GaugeServicePoint point in _points)
        {
            if (_handlers.TryGetValue(point, out Action handler))
            {
                point.OnServiceComplete -= handler;
            }
        }

        _handlers.Clear();
    }

    public override void BeginService(ContentUnit unit, GuestPresenter guest, Action onComplete)
    {
        if (!_pointByUnit.TryGetValue(unit, out GaugeServicePoint point))
        {
            Debug.LogError($"[{name}] '{unit.name}'에 GaugeServicePoint가 없어 서비스를 즉시 완료합니다.", unit);
            onComplete?.Invoke();
            return;
        }

        _pendingByPoint[point] = onComplete;
        point.ShowGauge();
    }

    private void HandleServiceComplete(GaugeServicePoint point)
    {
        if (!_pendingByPoint.TryGetValue(point, out Action complete))
        {
            return;
        }

        _pendingByPoint.Remove(point);
        point.HideGauge();

        complete?.Invoke();
    }

    // 카운터 직원이 하나씩 차지할 근무점을 배정. 유닛 수보다 직원이 많으면 null
    public Transform ClaimWorkPoint()
    {
        while (_claimedWorkPointCount < _points.Length)
        {
            GaugeServicePoint point = _points[_claimedWorkPointCount];
            _claimedWorkPointCount++;

            if (point.EmployeeWorkPoint != null)
            {
                return point.EmployeeWorkPoint;
            }
        }

        return null;
    }
}
