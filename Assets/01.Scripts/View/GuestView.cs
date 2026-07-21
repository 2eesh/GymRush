using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuestView : MonoBehaviour, IGuestView, IPoolable
{
    [SerializeField] private MoneyPileInstaller _moneyPilePrefab;

    private Rigidbody2D _rigidbody2d;
    private GuestPresenter _presenter;

    public Vector2 Position => transform.position;

    // 게스트는 어떤 게이지도 채우지 않음(응대는 Player/Employee 몫) — ICharacterView 계약상만 존재
    public float GuideGaugeRatePerSecond { get; set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _presenter?.Tick(Time.deltaTime);
    }

    // 스포너가 풀에서 꺼낸 직후 호출. 최초 1회만 MVP 조립, 이후엔 런 상태 리셋
    public void Setup()
    {
        _presenter.Setup();
    }
    
    public void Construct(GuestPresenter presenter)
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

    public void DropMoney(Vector2 position, int amount)
    {
        if (_moneyPilePrefab == null)
        {
            Debug.LogWarning($"[GuestView] {name}에 MoneyPile 프리팹이 연결되지 않았습니다.");
            return;
        }

        MoneyPileInstaller pile = Instantiate(_moneyPilePrefab, position, Quaternion.identity);
        pile.Setup(amount);
    }

    public void OnSpawn()
    {
        _rigidbody2d.linearVelocity = Vector2.zero;
    }

    public void OnDespawn()
    {
        _rigidbody2d.linearVelocity = Vector2.zero;
    }
}
