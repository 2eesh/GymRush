using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationControllerBase : MonoBehaviour, IStation
{
    [SerializeField] private Transform[] _queuePoints;
    [Tooltip("이 스테이션의 요금이 쌓이는 고정 MoneyPile. 씬에 활성 상태 + Start Count 0으로 배치하면 시작 직후 스스로 꺼짐")]
    [SerializeField] private MoneyPileView _moneyPile;
    [SerializeField] private int _serviceFee;
    [SerializeField] private float _serviceDuration = 3.0f;
    [Tooltip("해금(공사 완료) 시 켜지는 컨텐츠 루트. 지정하면 이 오브젝트가 켜져 있어야 이용 가능한 스테이션으로 취급. 처음부터 열려있는 스테이션은 비워두면 됨")]
    [SerializeField] private GameObject _contents;
    
    private readonly List<GuestPresenter> _line = new List<GuestPresenter>();
    private readonly Dictionary<GuestPresenter, IUsableSlot> _activeSlots = new Dictionary<GuestPresenter, IUsableSlot>();

    public int ServiceFee => _serviceFee;

    // 게스트가 던진 돈이 도착할 위치. 파일이 없으면 스테이션 자신의 위치로 대체
    public Vector3 MoneyPilePosition => _moneyPile != null ? _moneyPile.transform.position : transform.position;

    public void DepositMoney(int amount)
    {
        if (_moneyPile == null)
        {
            Debug.LogWarning($"[{name}] MoneyPile이 연결되지 않아 요금을 적립할 수 없습니다.");
            return;
        }

        _moneyPile.Deposit(amount);
    }

    // 손님/직원이 갈 수 있는 스테이션인지 여부.
    // 존 루트는 항상 켜져 있고(대기줄/공사존이 루트 밑에 있으므로) 해금 여부는 Contents 활성화로 표현되기 때문에,
    // 루트가 아니라 Contents가 켜져 있는지를 본다.
    public bool IsAvailable => gameObject.activeInHierarchy && (_contents == null || _contents.activeInHierarchy);
    
    protected virtual IUsableSlot[] Slots => Array.Empty<IUsableSlot>();

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

    protected bool IsFirstInLine(GuestPresenter guest)
    {
        return GetIndex(guest) == 0;
    }

    protected void RemoveFromLine(GuestPresenter guest)
    {
        _line.Remove(guest);
    }

    public virtual bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint)
    {
        usePoint = default;

        if (!IsFirstInLine(guest))
        {
            return false;
        }
        
        foreach (var slot in Slots)
        {
            if (slot.CanUse)
            {
                slot.StartUse();
                usePoint = slot.SeatPoint.position;
                RemoveFromLine(guest);
                _activeSlots[guest] = slot;
                return true;
            }
        }

        return false;
    }
    
    public virtual void BeginService(GuestPresenter guest)
    {
        StartCoroutine(ServiceRoutine(guest));
    }

    public virtual void FinishService(GuestPresenter guest)
    {
        if (_activeSlots.TryGetValue(guest, out IUsableSlot slot))
        {
            slot.FinishUse();
            _activeSlots.Remove(guest);
        }
    }
    
    private IEnumerator ServiceRoutine(GuestPresenter guest)
    {
        yield return new WaitForSeconds(_serviceDuration);

        FinishService(guest);

        guest.NotifyServiceComplete();
    }

   
}
