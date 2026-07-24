using UnityEngine;

public class GuestPresenter
{
    private const float ArriveDistance = 0.15f;

    private readonly GuestModel _model;
    private readonly IGuestView _view;
    private readonly GuestStateMachine _fsm;

    public GuestModel Model => _model;
    public IGuestView View => _view;
    public GuestContext Context { get; private set; }
    public GuestState CurrentStateId => _fsm.CurrentState != null ? _fsm.CurrentState.StateId : default;

    public IStation TargetStation { get; private set; }

    public GuestPresenter(GuestModel model, IGuestView view)
    {
        _model = model;
        _view = view;
        _fsm = new GuestStateMachine(this);
    }

    // 풀에서 재사용되므로 소속 스테이지의 컨텍스트를 스폰 시마다 다시 주입받는다
    public void Setup(GuestContext context)
    {
        Context = context;
        _model.Setup();
        _view.SetExpression(GuestExpression.Neutral);
        ClearTargetStation();
        ChangeState(GuestState.Enter);
    }

    public void GoToStation(IStation station)
    {
        TargetStation = station;
        ChangeState(GuestState.WaitInQueue);
    }
    
    public void ClearTargetStation()
    {
        TargetStation = null;
    }
    
    public void NotifyServiceComplete()
    {
        if (CurrentStateId == GuestState.UseStation)
        {
            ChangeState(GuestState.DropMoney);
        }
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
        _view.SetVelocity(_model.Direction * _model.MoveSpeed.Value);
        return false;
    }

    public void ChangeState(GuestState state)
    {
        _fsm.ChangeState(state);
    }
}
