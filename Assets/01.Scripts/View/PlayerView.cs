using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
{
    private Rigidbody2D _rigidbody2d;
    private PlayerPresenter _presenter;

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
