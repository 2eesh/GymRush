using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationControllerBase : MonoBehaviour, IStation
{
    [SerializeField] private Transform[] _queuePoints;
    [SerializeField] private Transform _moneyDropPoint;
    [SerializeField] private int _serviceFee;
    [SerializeField] private float _serviceDuration = 3.0f;
    
    private readonly List<GuestPresenter> _line = new List<GuestPresenter>();
    private readonly Dictionary<GuestPresenter, IUsableSlot> _activeSlots = new Dictionary<GuestPresenter, IUsableSlot>();

    public Transform MoneyDropPoint => _moneyDropPoint;
    public int ServiceFee => _serviceFee;
    
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
