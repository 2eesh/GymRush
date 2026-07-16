using UnityEngine;

public class PlayerView : MonoBehaviour, ICharacterView
{
    private Rigidbody2D _rigidbody2d;
    private PlayerPresenter _presenter;

    public float GuideGaugeRatePerSecond
    {
        get
        {
            return _presenter.GuideGaugeRatePerSecond;        
        }
        set
        {
            _presenter.GuideGaugeRatePerSecond = value;
        }
    }
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Construct(PlayerPresenter presenter)
    {
        _presenter = presenter;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rigidbody2d.linearVelocity = velocity;
    }

    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
