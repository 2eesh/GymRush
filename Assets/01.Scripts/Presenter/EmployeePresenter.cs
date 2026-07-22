using UnityEngine;

public class EmployeePresenter
{
    private const float ArriveDistance = 0.15f;

    private readonly EmployeeModel _model;
    private readonly IEmployeeView _view;
    private readonly IEmployeeJob _job;
    private readonly Vector2 _restPoint;
    private readonly EmployeeStateMachine _fsm;

    public EmployeeModel Model => _model;
    public IEmployeeView View => _view;
    public IEmployeeJob Job => _job;
    public Vector2 RestPoint => _restPoint;
    public EmployeeState CurrentStateId => _fsm.CurrentState != null ? _fsm.CurrentState.StateId : default;

    // 현재 작업 지점 (MoveToWork/Work 상태에서만 유효)
    public Vector2 WorkPoint { get; private set; }

    public EmployeePresenter(EmployeeModel model, IEmployeeView view, IEmployeeJob job, Vector2 restPoint)
    {
        _model = model;
        _view = view;
        _job = job;
        _restPoint = restPoint;
        _fsm = new EmployeeStateMachine(this);
    }

    public void Setup()
    {
        _model.Setup();
        _view.GuideGaugeRatePerSecond = _model.GuideGaugeRatePerSecond;
        ChangeState(EmployeeState.Idle);
    }

    // Job에 할 일이 있는지 물어보고, 있으면 작업 지점으로 출발
    public bool TryStartWork()
    {
        if (_job.TryGetWork(out Vector2 workPoint))
        {
            WorkPoint = workPoint;
            ChangeState(EmployeeState.MoveToWork);
            return true;
        }

        return false;
    }

    public void Tick(float deltaTime)
    {
        _fsm.Tick(deltaTime);
    }

    public bool MoveTowards(Vector2 target)
    {
        Vector2 toTarget = target - _view.Position;

        if (toTarget.magnitude <= ArriveDistance)
        {
            _model.Direction = Vector2.zero;
            _view.SetVelocity(Vector2.zero);
            return true;
        }

        _model.Direction = toTarget.normalized;
        _view.SetVelocity(_model.Direction * _model.MoveSpeed);
        return false;
    }

    public void ChangeState(EmployeeState state)
    {
        _fsm.ChangeState(state);
    }
}
