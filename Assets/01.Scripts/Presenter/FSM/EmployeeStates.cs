using UnityEngine;

public abstract class EmployeeStateBase : IState<EmployeeState>
{
    public abstract EmployeeState StateId { get; }

    protected readonly EmployeePresenter _owner;
    protected EmployeeModel Model => _owner.Model;
    protected IEmployeeView View => _owner.View;

    protected EmployeeStateBase(EmployeePresenter owner)
    {
        _owner = owner;
    }

    public virtual void Enter() { }
    public virtual void Tick(float deltaTime) { }
    public virtual void Exit() { }
}

// 대기 지점으로 돌아가며, 매 틱 Job에게 할 일이 생겼는지 물어봄
public class EmployeeIdleState : EmployeeStateBase
{
    public override EmployeeState StateId => EmployeeState.Idle;

    public EmployeeIdleState(EmployeePresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (_owner.TryStartWork())
        {
            return;
        }

        _owner.MoveTowards(_owner.RestPoint);
    }
}

// 작업 지점으로 이동. 가는 도중 일이 사라지면(예: 플레이어가 먼저 청소) 복귀
public class EmployeeMoveToWorkState : EmployeeStateBase
{
    public override EmployeeState StateId => EmployeeState.MoveToWork;

    public EmployeeMoveToWorkState(EmployeePresenter owner) : base(owner) { }

    public override void Tick(float deltaTime)
    {
        if (_owner.Job.IsWorkFinished())
        {
            _owner.ChangeState(EmployeeState.Idle);
            return;
        }

        if (_owner.MoveTowards(_owner.WorkPoint))
        {
            _owner.ChangeState(EmployeeState.Work);
        }
    }
}

// 작업 지점(게이지 존 내부)에 서 있음 — 게이지 충전은 존이 GuideGaugeRatePerSecond로 처리
public class EmployeeWorkState : EmployeeStateBase
{
    public override EmployeeState StateId => EmployeeState.Work;

    public EmployeeWorkState(EmployeePresenter owner) : base(owner) { }

    public override void Enter()
    {
        View.SetVelocity(Vector2.zero);
    }

    public override void Tick(float deltaTime)
    {
        if (_owner.Job.IsWorkFinished())
        {
            _owner.ChangeState(EmployeeState.Idle);
        }
    }
}
