using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class GuestView : MonoBehaviour, IGuestView, IPoolable
{
    [Tooltip("요금을 지불할 때 게스트가 던지는 연출용 프리팹(돈 비주얼). 비워두면 연출 없이 즉시 적립")]
    [FormerlySerializedAs("_moneyThrowEffectPrefab")]
    [SerializeField] private ProjectileEffect _throwEffectPrefab;

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

    // 자기 위치에서 목표 지점으로 돈을 던지는 연출. 도착 시 onArrived 호출(적립 시점)
    public void ThrowMoney(Vector3 target, Action onArrived)
    {
        if (_throwEffectPrefab == null)
        {
            onArrived?.Invoke();
            return;
        }

        ProjectileEffect effect = PoolManager.Instance.Get(_throwEffectPrefab, transform.position, Quaternion.identity);
        effect.Play(target, onArrived);
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
