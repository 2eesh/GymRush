using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 업그레이드 패널의 직원 한 명 카드 — 스탯 2종(스피드/처리속도)의 레벨 게이지와 구매 버튼 표시
public class UIEmployeeUpgradeEntryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    [Header("스피드")]
    [SerializeField] private Image _speedGauge;
    [SerializeField] private TextMeshProUGUI _speedLevelText;
    [SerializeField] private Button _speedButton;
    [SerializeField] private TextMeshProUGUI _speedCostText;

    [Header("처리속도")]
    [SerializeField] private Image _workRateGauge;
    [SerializeField] private TextMeshProUGUI _workRateLevelText;
    [SerializeField] private Button _workRateButton;
    [SerializeField] private TextMeshProUGUI _workRateCostText;

    public event Action<EmployeeStatType> OnUpgradeClicked;

    private void Awake()
    {
        _speedButton.onClick.AddListener(() => OnUpgradeClicked?.Invoke(EmployeeStatType.Speed));
        _workRateButton.onClick.AddListener(() => OnUpgradeClicked?.Invoke(EmployeeStatType.WorkRate));
    }

    public void SetName(string employeeName)
    {
        _nameText.text = employeeName;
    }

    public void SetStat(EmployeeStatType statType, int level, int maxLevel, int cost, bool canAfford)
    {
        Image gauge = statType == EmployeeStatType.Speed ? _speedGauge : _workRateGauge;
        TextMeshProUGUI levelText = statType == EmployeeStatType.Speed ? _speedLevelText : _workRateLevelText;
        TextMeshProUGUI costText = statType == EmployeeStatType.Speed ? _speedCostText : _workRateCostText;
        Button button = statType == EmployeeStatType.Speed ? _speedButton : _workRateButton;

        bool isMax = level >= maxLevel;
        gauge.fillAmount = (float)level / maxLevel;
        levelText.text = $"Lv{level}";
        costText.text = isMax ? "MAX" : cost.ToString();
        button.interactable = !isMax && canAfford;
    }
}
