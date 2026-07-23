using UnityEngine;

public interface IStation
{
    int ServiceFee { get; }
    bool IsAvailable { get; }

    void DepositMoney(int amount);

    int Enqueue(GuestPresenter guest);
    int GetIndex(GuestPresenter guest);
    Transform GetQueuePoint(int index);
    
    bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint);

    void BeginService(GuestPresenter guest);
    void FinishService(GuestPresenter guest);
    
    void CancelWait(GuestPresenter guest);
}
