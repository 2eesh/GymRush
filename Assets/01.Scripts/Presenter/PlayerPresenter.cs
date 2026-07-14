using UnityEngine;

public class PlayerPresenter
{
    private readonly PlayerModel _model;
    private readonly IPlayerView _view;

    public PlayerPresenter(PlayerModel model, IPlayerView view)
    {
        _model = model;
        _view = view;
    }

    public void UpdateDirection(Vector2 direction)
    {
        _model.Direction = direction;
        
        UpdateVelocity();
    }
    
    private void UpdateVelocity()
    {
        Vector3 velocity = _model.Direction.normalized * _model.MoveSpeed;
        _view.SetVelocity(velocity);
    }
}
