using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoneyPileView : MonoBehaviour, IMoneyPileView
{
    [SerializeField] private GameObject _money1;
    [SerializeField] private GameObject _money2;
    [SerializeField] private GameObject _money4;
    [SerializeField] private GameObject _money8;
    [SerializeField] private string _interactorTag = "Player";
    [SerializeField] private float _collectDuration = 0.4f;
    [SerializeField] private float _arcHeight = 1.5f;

    private Collider2D _collider;
    private MoneyPilePresenter _presenter;
    private Vector3 _originPosition;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _originPosition = transform.position;
    }

    public void Construct(MoneyPilePresenter presenter)
    {
        _presenter = presenter;
    }

    public void UpdateVisual(int count)
    {
        _money1.SetActive(count >= 1 && count < 2);
        _money2.SetActive(count >= 2 && count < 4);
        _money4.SetActive(count >= 4 && count < 8);
        _money8.SetActive(count >= 8);
    }

    // 게스트가 요금을 놓을 때 스테이션이 호출하는 진입점
    public void Deposit(int amount)
    {
        if (_presenter == null)
        {
            Debug.LogWarning($"[{name}] 아직 조립되지 않아 요금을 적립할 수 없습니다. 씬에서 활성 상태로 배치했는지 확인하세요.");
            return;
        }

        _presenter.Deposit(amount);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PlayCollectEffect(Transform target, Action onArrived)
    {
        _collider.enabled = false;
        StartCoroutine(FlyToTargetRoutine(target, onArrived));
    }

    private IEnumerator FlyToTargetRoutine(Transform target, Action onArrived)
    {
        Vector3 start = transform.position;
        Vector3 direction = target.position - start;
        Vector3 arcOffset = new Vector3(-direction.y, direction.x, 0f).normalized * _arcHeight;
        Vector3 controlPoint = start + direction * 0.5f + arcOffset;

        float elapsed = 0f;
        while (elapsed < _collectDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _collectDuration);

            Vector3 end = target.position;
            Vector3 a = Vector3.Lerp(start, controlPoint, t);
            Vector3 b = Vector3.Lerp(controlPoint, end, t);
            transform.position = Vector3.Lerp(a, b, t);

            yield return null;
        }

        // 파괴하지 않고 원위치로 되돌려 재사용 — 활성/비활성 결정은 Presenter 몫
        transform.position = _originPosition;
        _collider.enabled = true;
        onArrived?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(_interactorTag))
        {
            return;
        }

        var receiver = other.GetComponent<ICurrencyReceiver>();
        if (receiver == null)
        {
            Debug.LogWarning($"{other.name}에 ICurrencyReceiver 구현체가 없습니다.");
            return;
        }

        _presenter.Collect(receiver, other.transform);
    }
}
