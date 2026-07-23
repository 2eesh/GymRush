using UnityEngine;

public interface IStation
{
    Transform MoneyDropPoint { get; }
    int ServiceFee { get; }
    bool IsAvailable { get; }

    int Enqueue(GuestPresenter guest);
    int GetIndex(GuestPresenter guest);
    Transform GetQueuePoint(int index);
    
    bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint);

    void BeginService(GuestPresenter guest);
    void FinishService(GuestPresenter guest);
    
    void CancelWait(GuestPresenter guest);
}
