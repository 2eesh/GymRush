using System;
using UnityEngine;

public interface IStation
{
    event Action<GuestPresenter> OnServiceComplete;

    Transform MoneyDropPoint { get; }
    int ServiceFee { get; }

    int Enqueue(GuestPresenter guest);
    int GetIndex(GuestPresenter guest);
    Transform GetQueuePoint(int index);

    // 선두(index 0)이고 빈 슬롯이 있을 때만 성공. 성공 시 대기열에서 제거됨
    bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint);

    void BeginService(GuestPresenter guest);

    // 대기 중 이탈 시 정리(안전망)
    void CancelWait(GuestPresenter guest);
}
