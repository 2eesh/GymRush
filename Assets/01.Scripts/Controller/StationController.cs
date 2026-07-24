using System.Collections.Generic;
using UnityEngine;

// 스테이션의 "손님을 어디에 세우고 어디에 앉히는가" — 대기열과 슬롯 배정만 담당한다.
// 서비스·요금·해금은 전부 StationContents 책임이며, 이 클래스는 손님 FSM이 보는 IStation 파사드다.
public class StationController : MonoBehaviour, IStation
{
    [SerializeField] private Transform[] _queuePoints;
    [SerializeField] private StationContents _contents;

    private readonly List<GuestPresenter> _line = new List<GuestPresenter>();

    // 이용 중인 손님 → 유닛
    private readonly Dictionary<GuestPresenter, ContentUnit> _activeUnits = new Dictionary<GuestPresenter, ContentUnit>();

    // 서비스 종료 후 요금 적립(돈 던지기 콜백)까지 유닛을 기억 — FinishService가 돈 던지기보다 먼저 일어나기 때문
    private readonly Dictionary<GuestPresenter, ContentUnit> _lastUnits = new Dictionary<GuestPresenter, ContentUnit>();

    public StationContents Contents => _contents;

    public bool IsAvailable => gameObject.activeInHierarchy && _contents.HasUnlockedUnit;

    public bool HasQueueSpace => _line.Count < _queuePoints.Length;

    public int Enqueue(GuestPresenter guest)
    {
        _line.Add(guest);
        return _line.IndexOf(guest);
    }

    public int GetIndex(GuestPresenter guest)
    {
        return _line.IndexOf(guest);
    }

    public Transform GetQueuePoint(int index)
    {
        int clamped = Mathf.Clamp(index, 0, _queuePoints.Length - 1);
        return _queuePoints[clamped];
    }

    public void CancelWait(GuestPresenter guest)
    {
        _line.Remove(guest);
    }

    public bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint)
    {
        usePoint = default;

        if (GetIndex(guest) != 0)
        {
            return false;
        }

        if (!_contents.TryClaimUnit(out ContentUnit unit, out usePoint))
        {
            return false;
        }

        _line.Remove(guest);
        _activeUnits[guest] = unit;
        return true;
    }

    public void BeginService(GuestPresenter guest)
    {
        if (!_activeUnits.TryGetValue(guest, out ContentUnit unit))
        {
            Debug.LogWarning($"[{name}] 슬롯 배정 기록이 없는 손님의 서비스 요청을 무시합니다.");
            return;
        }

        _contents.BeginService(unit, guest, () =>
        {
            FinishService(guest);
            guest.NotifyServiceComplete();
        });
    }

    public void FinishService(GuestPresenter guest)
    {
        if (_activeUnits.TryGetValue(guest, out ContentUnit unit))
        {
            unit.Slot?.FinishUse();
            _activeUnits.Remove(guest);
            _lastUnits[guest] = unit;
        }
    }

    public int GetServiceFee(GuestPresenter guest)
    {
        return _lastUnits.TryGetValue(guest, out ContentUnit unit) ? unit.ServiceFee : 0;
    }

    public Vector3 GetMoneyPilePosition(GuestPresenter guest)
    {
        return _lastUnits.TryGetValue(guest, out ContentUnit unit) ? unit.MoneyPilePosition : transform.position;
    }

    public void DepositMoney(GuestPresenter guest, int amount)
    {
        if (_lastUnits.TryGetValue(guest, out ContentUnit unit))
        {
            _lastUnits.Remove(guest);
            unit.DepositMoney(amount);
        }
        else
        {
            Debug.LogWarning($"[{name}] 손님의 마지막 이용 유닛 기록이 없어 요금을 적립하지 못했습니다.");
        }
    }
}
