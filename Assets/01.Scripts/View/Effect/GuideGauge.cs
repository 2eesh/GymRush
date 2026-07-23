using System;
using UnityEngine;
using UnityEngine.UI;

public class GuideGauge : MonoBehaviour
{
     [SerializeField] private Image _fillImage;

    public event Action OnGaugeComplated;
    public bool IsGaugeComplated { get; set; } = false;
    private float _currentProgress = 0f;

    public void AddProgress(float delta)
    {
        if (IsGaugeComplated)
        {
            return;
        }

        float clamped = Mathf.Clamp01(_currentProgress + delta);
        _currentProgress = clamped;

        if (_fillImage != null)
        {
            _fillImage.fillAmount = clamped;
        }

        CheckCompletion();
    }
    
    public void ResetGauge()
    {
        _currentProgress = 0f;
        if (_fillImage != null)
        {
            _fillImage.fillAmount = 0f;
        }
        
        IsGaugeComplated = false;
        
    }
    
    private void CheckCompletion()
    {
        if (_currentProgress >= 1f)
        {
            IsGaugeComplated = true;
            OnGaugeComplated?.Invoke();
        }
    }
}
