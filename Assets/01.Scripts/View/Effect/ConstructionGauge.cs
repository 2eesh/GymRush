using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionGauge : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _costText;
    [Tooltip("공사 비용. UnlockChainData에 항목이 있으면 시작 시 그 값으로 덮어써지고, 없을 때만 이 값이 사용됨")]
    [SerializeField] private int _cost;

    public event Action OnConstructionComplete;
    public bool IsComplete { get; private set; } = false;
    private int _currentAmount = 0;

    public void SetCost(int cost)
    {
        if (IsComplete)
        {
            return;
        }

        _cost = cost;
        RefreshUI();
    }

    public int AddFunds(int amount)
    {
        if (IsComplete)
        {
            return 0;
        }

        int applied = Mathf.Clamp(amount, 0, _cost - _currentAmount);
        _currentAmount += applied;

        RefreshUI();
        CheckCompletion();

        return applied;
    }

    private void RefreshUI()
    {
        if (_slider != null)
        {
            _slider.value = _cost > 0 ? (float)_currentAmount / _cost : 0f;
        }

        if (_costText != null)
        {
            _costText.text = (_cost - _currentAmount).ToString();
        }
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
