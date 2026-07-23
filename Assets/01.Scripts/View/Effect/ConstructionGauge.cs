using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionGauge : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private int _cost;

    public event Action OnConstructionComplete;
    public bool IsComplete { get; private set; } = false;
    private int _currentAmount = 0;

    public int AddFunds(int amount)
    {
        if (IsComplete)
        {
            return 0;
        }

        int applied = Mathf.Clamp(amount, 0, _cost - _currentAmount);
        _currentAmount += applied;

        if (_slider != null)
        {
            _slider.value = (float)_currentAmount / _cost;
        }

        if (_costText != null)
        {
            _costText.text = (_cost - _currentAmount).ToString();
        }

        CheckCompletion();

        return applied;
    }

    private void CheckCompletion()
    {
        if (_currentAmount >= _cost)
        {
            IsComplete = true;
            OnConstructionComplete?.Invoke();
        }
    }
}
