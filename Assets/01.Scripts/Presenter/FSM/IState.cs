public interface IState<TStateId>
{
    TStateId StateId { get; }
    void Enter();
    void Tick(float deltaTime);
    void Exit();
}
