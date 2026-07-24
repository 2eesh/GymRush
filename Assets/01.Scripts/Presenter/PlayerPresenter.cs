using UnityEngine;

public class PlayerPresenter
{
    private readonly PlayerModel _model;
    private readonly ICharacterView _view;

    public float GuideGaugeRatePerSecond
    {
        get
        {
            return _model.GuideGaugeRatePerSecond.Value;
        }
        set
        {
            _model.GuideGaugeRatePerSecond.BaseValue = value;
        }
    }

    public PlayerPresenter(PlayerModel model, ICharacterView view)
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
        Vector3 velocity = _model.Direction.normalized * _model.MoveSpeed.Value;
        _view.SetVelocity(velocity);
    }
}
