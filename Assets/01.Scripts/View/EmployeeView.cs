using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EmployeeView : MonoBehaviour, IEmployeeView
{
    private Rigidbody2D _rigidbody2d;
    private EmployeePresenter _presenter;

    public Vector2 Position => transform.position;

    // 게이지 존(GuideInterationGaugeZoneController)이 읽어가는 초당 게이지 충전량
    public float GuideGaugeRatePerSecond { get; set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _presenter?.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _presenter?.Dispose();
    }

    public void Construct(EmployeePresenter presenter)
    {
        _presenter = presenter;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rigidbody2d.linearVelocity = velocity;
    }

    public void SetPosition(Vector2 position)
    {
        _rigidbody2d.position = position;
    }
}
