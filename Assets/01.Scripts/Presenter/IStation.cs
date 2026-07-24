using UnityEngine;

public interface IStation
{
    bool IsAvailable { get; }

    // 대기줄에 빈 자리가 있는가 — 정원은 대기 포인트 개수
    bool HasQueueSpace { get; }

    // 요금 액수·적립처 모두 손님이 마지막으로 이용한 유닛 기준이므로 guest가 필요
    int GetServiceFee(GuestPresenter guest);
    Vector3 GetMoneyPilePosition(GuestPresenter guest);

    void DepositMoney(GuestPresenter guest, int amount);

    int Enqueue(GuestPresenter guest);
    int GetIndex(GuestPresenter guest);
    Transform GetQueuePoint(int index);

    bool TryClaimSlot(GuestPresenter guest, out Vector3 usePoint);

    void BeginService(GuestPresenter guest);
    void FinishService(GuestPresenter guest);

    void CancelWait(GuestPresenter guest);
}
