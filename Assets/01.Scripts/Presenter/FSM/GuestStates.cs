using UnityEngine;

public abstract class GuestStateBase : IState<GuestState>
{
    public abstract GuestState StateId { get; }

    protected readonly GuestPresenter _owner;
    protected GuestModel Model => _owner.Model;
    protected IGuestView View => _owner.View;

    protected GuestStateBase(GuestPresenter owner)
    {
        _owner = owner;
    }

    public virtual void Enter() { }
    public virtual void Tick(float deltaTime) { }
    public virtual void Exit() { }
}

// 입구에서 스폰되어 짐 안(InsidePoint)으로 이동
public class GuestEnterState : GuestStateBase
{
    public override GuestState StateId => GuestState.Enter;

    public GuestEnterState(GuestPresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (_owner.MoveTowards(_owner.Context.InsidePoint.position))
        {
            _owner.ChangeState(GuestState.DecideNext);
        }
    }
}

// 허브 — 우선순위(카운터 → 탈의실 → 운동기구 → 퇴장)대로 다음 목표를 정함
public class GuestDecideNextState : GuestStateBase
{
    public override GuestState StateId => GuestState.DecideNext;

    public GuestDecideNextState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        // 잠긴(비활성) 존의 스테이션은 목적지 후보에서 제외 — 해금 전 구역으로 걸어가는 것 방지
        if (!Model.HasPaidCounter && _owner.Context.CounterStation.IsAvailable)
        {
            _owner.GoToStation(_owner.Context.CounterStation);
            return;
        }

        if (!Model.HasChangedClothes && _owner.Context.LockerStation.IsAvailable)
        {
            _owner.GoToStation(_owner.Context.LockerStation);
            return;
        }

        if (!Model.HasExercised)
        {
            IStation station = PickRandomAvailableStation(_owner.Context.EquipmentStations);

            if (station != null)
            {
                _owner.GoToStation(station);
                return;
            }

            // 이용 가능한 기구가 하나도 없으면 운동 단계를 건너뜀 (영구 정지 방지)
            Model.HasExercised = true;
        }

        _owner.ChangeState(GuestState.Exit);
    }

    private static IStation PickRandomAvailableStation(IStation[] stations)
    {
        if (stations == null || stations.Length == 0)
        {
            return null;
        }

        var available = new System.Collections.Generic.List<IStation>(stations.Length);
        foreach (IStation station in stations)
        {
            if (station.IsAvailable)
            {
                available.Add(station);
            }
        }

        return available.Count > 0 ? available[Random.Range(0, available.Count)] : null;
    }
}

// 존의 대기줄에 서서 순서를 기다리다가, 선두이고 빈 슬롯이 나면 그 슬롯으로 이동
public class GuestWaitInQueueState : GuestStateBase
{
    public override GuestState StateId => GuestState.WaitInQueue;

    private bool _enqueued;
    private bool _movingToSlot;
    private Vector2 _slotPoint;

    public GuestWaitInQueueState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        _movingToSlot = false;

        // 대기줄이 꽉 찼으면 줄을 서지 않고 짜증을 내며 퇴장
        if (!_owner.TargetStation.HasQueueSpace)
        {
            _enqueued = false;
            View.SetExpression(GuestExpression.Annoyed);
            _owner.ClearTargetStation();
            _owner.ChangeState(GuestState.Exit);
            return;
        }

        _owner.TargetStation.Enqueue(_owner);
        _enqueued = true;
    }

    public override void Tick(float deltaTime)
    {
        if (_movingToSlot)
        {
            if (_owner.MoveTowards(_slotPoint))
            {
                _owner.ChangeState(GuestState.UseStation);
            }

            return;
        }

        IStation station = _owner.TargetStation;
        int index = station.GetIndex(_owner);

        if (index < 0)
        {
            return;
        }

        if (index == 0 && station.TryClaimSlot(_owner, out Vector3 usePoint))
        {
            _enqueued = false;
            _movingToSlot = true;
            _slotPoint = usePoint;
            return;
        }

        _owner.MoveTowards(station.GetQueuePoint(index).position);
    }

    public override void Exit()
    {
        base.Exit();

        if (_enqueued)
        {
            _owner.TargetStation.CancelWait(_owner);
            _enqueued = false;
        }
    }
}

// 슬롯에서 서비스 이용 — 스테이션이 NotifyServiceComplete로 알려줄 때까지 대기
public class GuestUseStationState : GuestStateBase
{
    public override GuestState StateId => GuestState.UseStation;

    public GuestUseStationState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        View.SetVelocity(Vector2.zero);
        _owner.TargetStation.BeginService(_owner);
    }
}

// 방금 이용한 존의 드랍 위치에 그 존의 요금을 놓고, 방문 기억을 갱신
public class GuestDropMoneyState : GuestStateBase
{
    public override GuestState StateId => GuestState.DropMoney;

    private const float DropDelay = 0.3f;
    private float _elapsed;

    public GuestDropMoneyState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        _elapsed = 0f;

        IStation station = _owner.TargetStation;
        View.ThrowMoney(station.GetMoneyPilePosition(_owner), () => station.DepositMoney(_owner, station.GetServiceFee(_owner)));

        // 방금 마친 스테이션에 해당하는 방문 기억을 켬
        if (station == _owner.Context.CounterStation)
        {
            Model.HasPaidCounter = true;
        }
        else if (station == _owner.Context.LockerStation)
        {
            Model.HasChangedClothes = true;
        }
        else
        {
            Model.HasExercised = true;
            View.SetExpression(GuestExpression.Happy);
        }

        _owner.ClearTargetStation();
    }

    public override void Tick(float deltaTime)
    {
        _elapsed += deltaTime;

        if (_elapsed >= DropDelay)
        {
            _owner.ChangeState(GuestState.DecideNext);
        }
    }
}

// 출구로 걸어가 도착하면 스포너에게 알려 풀로 반환
public class GuestExitState : GuestStateBase
{
    public override GuestState StateId => GuestState.Exit;

    public GuestExitState(GuestPresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (_owner.MoveTowards(_owner.Context.ExitPoint.position))
        {
            PoolManager.Instance.Return(View as GuestView);
        }
    }
}
