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

    public void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            _model.Direction = direction;
        }
        
        UpdateRotation();
        UpdateVelocity(direction);
    }

    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(_model.Direction.y, _model.Direction.x) * Mathf.Rad2Deg;
        _view.SetRotation(angle);
    }

    private void UpdateVelocity(Vector3 direction)
    {
        Vector3 velocity = direction.normalized * _model.MoveSpeed;
        _view.SetVelocity(velocity);
    }
}
