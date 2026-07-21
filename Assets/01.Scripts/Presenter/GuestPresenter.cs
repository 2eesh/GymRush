using System;
using UnityEngine;

public class GuestPresenter
{
    private readonly GuestModel _model;
    private readonly IGuestView _view;
    private readonly GuestContext _context;
    private readonly GuestStateMachine _fsm;

    public GuestModel Model => _model;
    public IGuestView View => _view;
    public GuestContext Context => _context;
    public GuestState CurrentStateId => _fsm.CurrentState != null ? _fsm.CurrentState.StateId : default;

    // DecideNext가 정하고 이후 상태들이 사용하는 이동/서비스 목표
    public IStation TargetStation { get; set; }
    public Vector2 TargetPosition { get; set; }
    public GuestState AfterMoveState { get; set; }
    public Action OnStationCycleComplete { get; set; }

    public GuestPresenter(GuestModel model, IGuestView view, GuestContext context)
    {
        _model = model;
        _view = view;
        _context = context;
        _fsm = new GuestStateMachine(this);
    }

    // 스폰(재사용 포함)마다 호출 — 방문 주기 리셋 후 Enter부터 시작
    public void Setup()
    {
        _model.Setup();
        TargetStation = null;
        OnStationCycleComplete = null;
        ChangeState(GuestState.Enter);
    }

    public void Tick(float deltaTime)
    {
        _fsm.Tick(deltaTime);
    }

    public void ChangeState(GuestState state)
    {
        _fsm.ChangeState(state);
    }
}
