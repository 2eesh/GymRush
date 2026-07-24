using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ConstructionInteractionZoneController : MonoBehaviour
{
    [SerializeField] private string _unlockId;
    [Tooltip("해금 대기 중일 때 통째로 숨길 루트(예: ConstructionMaker). 비워두면 이 오브젝트 자신을 숨김")]
    [SerializeField] private GameObject _zoneRoot;
    [SerializeField] private ConstructionGauge _constructionGauge;
    [SerializeField] private string[] _interactorTags = { "Player" };
    [SerializeField] private float _spendRatePerSecond = 100f;
    [SerializeField] private GameObject[] _revealOnComplete;
    [SerializeField] private GameObject[] _hideOnComplete;

    private readonly Dictionary<Collider2D, ICurrencySpender> _occupants = new Dictionary<Collider2D, ICurrencySpender>();
    private Coroutine _fillRoutine;

    private bool IsOccupied => _occupants.Count > 0;

    public string UnlockId => _unlockId;
    public GameObject ZoneRoot => _zoneRoot != null ? _zoneRoot : gameObject;

    public void SetCost(int cost) => _constructionGauge.SetCost(cost);

    private void OnEnable()
    {
        _constructionGauge.OnConstructionComplete += HandleComplete;
    }

    private void OnDisable()
    {
        _constructionGauge.OnConstructionComplete -= HandleComplete;
    }

    private void HandleComplete()
    {
        Debug.Log($"HandleComplete: {_unlockId}");
        foreach (var target in _hideOnComplete)
        {
            target.SetActive(false);
        }

        foreach (var target in _revealOnComplete)
        {
            target.SetActive(true);
        }

        UnlockChainManager.Instance.ReportComplete(_unlockId);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInteractor(other))
        {
            return;
        }
        
        var spender = other.GetComponent<ICurrencySpender>();
        if (spender == null)
        {
            Debug.LogWarning($"{other.name}에 ICurrencySpender 구현체가 없습니다.");
            return;
        }
        
        _occupants[other] = spender;

        if (_fillRoutine == null)
        {
            _fillRoutine = StartCoroutine(FillRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsInteractor(other))
        {
            return;
        }

        _occupants.Remove(other);
    }

    private IEnumerator FillRoutine()
    {
        while (IsOccupied && !_constructionGauge.IsComplete)
        {
            foreach (var spender in _occupants.Values)
            {
                int want = Mathf.Min(Mathf.RoundToInt(_spendRatePerSecond * Time.deltaTime), spender.Amount);
                if (want <= 0)
                {
                    continue;
                }
                
                int applied = _constructionGauge.AddFunds(want);
                if (applied > 0)
                {
                    spender.SpendMoney(applied);
                }

                if (_constructionGauge.IsComplete)
                {
                    break;
                }
            }

            yield return null;
        }
        
        _fillRoutine = null;
    }

    private bool IsInteractor(Collider2D other)
    {
        foreach (var tag in _interactorTags)
        {
            if (other.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }
}
