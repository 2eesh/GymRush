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

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
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

        onArrived?.Invoke();
        Destroy(gameObject);
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
