using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GuideInterationGaugeZoneController : MonoBehaviour
{
    [SerializeField] private GuideGauge _guideGauge;
    [SerializeField] private string[] _interactorTags = { "Player", "Employee" };

    public event Action OnGaugeComplete;
    
    private readonly Dictionary<Collider2D, ICharacterView> _occupants = new Dictionary<Collider2D, ICharacterView>();
    private Coroutine _fillRoutine;
    
    private bool IsOccupied => _occupants.Count > 0;
    private float TotalFillRatePerSecond => _occupants.Values.Sum(filler => filler.GuideGaugeRatePerSecond);

    private void OnEnable()
    {
        _guideGauge.OnGaugeComplated += CheckComplation;
        _guideGauge.ResetGauge();
    }

    private void OnDisable()
    {
        _guideGauge.OnGaugeComplated -= CheckComplation;
        _occupants.Clear();
        _fillRoutine = null;
    }

    private void CheckComplation()
    {
        OnGaugeComplete?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInteractor(other))
        {
            return;
        }
        
        var filler = other.GetComponent<ICharacterView>();
        if (filler == null)
        {
            Debug.LogWarning($"{other.name}에 ICharacterView 구현체가 없습니다.");
            return;
        }
        
        _occupants[other] = filler;
        
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
        while (IsOccupied)
        {
            _guideGauge.AddProgress(TotalFillRatePerSecond * Time.deltaTime);
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
