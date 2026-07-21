using UnityEngine;

public abstract class GuestStateBase : IState<GuestState>
{
    public abstract GuestState StateId { get; }
    protected const float ArriveDistance = 0.15f;

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

    // 목표 지점으로 직선 이동. 도착하면 정지 후 true 반환
    protected bool MoveTowards(Vector2 target)
    {
        Vector2 toTarget = target - View.Position;

        if (toTarget.magnitude <= ArriveDistance)
        {
            Model.Direction = Vector2.zero;
            View.SetVelocity(Vector2.zero);
            return true;
        }

        Model.Direction = toTarget.normalized;
        View.SetVelocity(Model.Direction * Model.MoveSpeed);
        return false;
    }
}

// 입구에서 스폰되어 짐 안(InsidePoint)으로 이동
public class GuestEnterState : GuestStateBase
{
    public override GuestState StateId => GuestState.Enter;

    public GuestEnterState(GuestPresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (MoveTowards(_owner.Context.InsidePoint.position))
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
        if (!Model.HasPaidCounter)
        {
            GoToStation(_owner.Context.CounterStation, () => Model.HasPaidCounter = true);
            return;
        }

        if (!Model.HasChangedClothes)
        {
            GoToStation(_owner.Context.LockerStation, () => Model.HasChangedClothes = true);
            return;
        }

        if (!Model.HasExercised)
        {
            IStation[] stations = _owner.Context.EquipmentStations;

            if (stations != null && stations.Length > 0)
            {
                IStation station = stations[Random.Range(0, stations.Length)];
                GoToStation(station, () => Model.HasExercised = true);
            }
            
            return;
        }

        _owner.TargetPosition = _owner.Context.ExitPoint.position;
        _owner.AfterMoveState = GuestState.Exit;
        _owner.ChangeState(GuestState.MoveToZone);
    }

    private void GoToStation(IStation station, System.Action onComplete)
    {
        _owner.TargetStation = station;
        _owner.OnStationCycleComplete = onComplete;
        _owner.ChangeState(GuestState.WaitInQueue);
    }
}

// 목표 지점으로 이동 후 AfterMoveState로 전이
public class GuestMoveToZoneState : GuestStateBase
{
    public override GuestState StateId => GuestState.MoveToZone;

    public GuestMoveToZoneState(GuestPresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (MoveTowards(_owner.TargetPosition))
        {
            _owner.ChangeState(_owner.AfterMoveState);
        }
    }
}

// 존의 대기줄에 서서 순서를 기다리다가, 선두이고 빈 슬롯이 나면 그 슬롯으로 이동
public class GuestWaitInQueueState : GuestStateBase
{
    public override GuestState StateId => GuestState.WaitInQueue;

    private bool _enqueued;

    public GuestWaitInQueueState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        _owner.TargetStation.Enqueue(_owner);
        _enqueued = true;
    }

    public override void Tick(float deltaTime)
    {
        IStation station = _owner.TargetStation;
        int index = station.GetIndex(_owner);

        if (index < 0)
        {
            return;
        }
        
        if (index == 0 && station.TryClaimSlot(_owner, out Vector3 usePoint))
        {
            _enqueued = false;
            _owner.TargetPosition = usePoint;
            _owner.AfterMoveState = GuestState.UseStation;
            _owner.ChangeState(GuestState.MoveToZone);
            return;
        }

        MoveTowards(station.GetQueuePoint(index).position);
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

// 슬롯에서 서비스 이용 — 완료 이벤트가 오면 결제로 전이 (카운터=게이지, 락커/기구=타이머)
public class GuestUseStationState : GuestStateBase
{
    public override GuestState StateId => GuestState.UseStation;

    private IStation _station;

    public GuestUseStationState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        View.SetVelocity(Vector2.zero);

        _station = _owner.TargetStation;
        _station.OnServiceComplete += HandleServiceComplete;
        _station.BeginService(_owner);
    }

    private void HandleServiceComplete(GuestPresenter guest)
    {
        if (guest != _owner)
        {
            return;
        }

        _owner.ChangeState(GuestState.DropMoney);
    }

    public override void Exit()
    {
        base.Exit();
        _station.OnServiceComplete -= HandleServiceComplete;
    }
}

// 방금 이용한 존의 드랍 위치에 그 존의 요금을 놓기
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
        View.DropMoney(station.MoneyDropPoint.position, station.ServiceFee);

        _owner.OnStationCycleComplete?.Invoke();
        _owner.OnStationCycleComplete = null;
        _owner.TargetStation = null;
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

// 출구 도착 — 스포너에게 알려 풀로 반환
public class GuestExitState : GuestStateBase
{
    public override GuestState StateId => GuestState.Exit;

    public GuestExitState(GuestPresenter owner) : base(owner) { }

    public override void Enter()
    {
        View.SetVelocity(Vector2.zero);
        PoolManager.Instance.Return(View as GuestView);
    }
}
