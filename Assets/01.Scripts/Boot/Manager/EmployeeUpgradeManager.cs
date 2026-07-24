using System;
using UnityEngine;

// 직원 업그레이드의 단일 진입점. 레벨 데이터는 EmployeeUpgradeModel이 들고 있고,
// 여기서는 씬 접근(싱글톤)과 비용 지불을 포함한 업그레이드 절차를 담당한다.
public class EmployeeUpgradeManager : SingletonMonoBehaviour<EmployeeUpgradeManager>
{
    public EmployeeUpgradeModel Upgrades { get; private set; }

    // (직원 Id, 스탯 종류, 새 레벨) — 모델 이벤트를 그대로 중계
    public event Action<string, EmployeeStatType, int> OnLevelChanged;

    protected override void Awake()
    {
        base.Awake();

        Upgrades = new EmployeeUpgradeModel();
        Upgrades.OnLevelChanged += (employeeId, statType, level) => OnLevelChanged?.Invoke(employeeId, statType, level);
    }

    public int GetLevel(string employeeId, EmployeeStatType statType)
    {
        return Upgrades.GetLevel(employeeId, statType);
    }

    public bool IsMaxLevel(string employeeId, EmployeeStatType statType)
    {
        return Upgrades.IsMaxLevel(employeeId, statType);
    }

    public float GetMultiplier(string employeeId, EmployeeStatType statType)
    {
        return Upgrades.GetMultiplier(employeeId, statType);
    }

    public int GetUpgradeCost(string employeeId, EmployeeStatType statType)
    {
        return Upgrades.GetUpgradeCost(employeeId, statType);
    }

    public bool TryUpgrade(string employeeId, EmployeeStatType statType, ICurrencySpender spender)
    {
        if (Upgrades.IsMaxLevel(employeeId, statType))
        {
            return false;
        }

        int cost = Upgrades.GetUpgradeCost(employeeId, statType);
        if (spender.Amount < cost)
        {
            Debug.Log($"[EmployeeUpgrade] 잔액 부족 — {employeeId} {statType} 비용 {cost}, 보유 {spender.Amount}");
            return false;
        }

        if (!Upgrades.TryLevelUp(employeeId, statType))
        {
            return false;
        }

        spender.SpendMoney(cost);
        Debug.Log($"[EmployeeUpgrade] {employeeId} {statType} → Lv{Upgrades.GetLevel(employeeId, statType)} (x{Upgrades.GetMultiplier(employeeId, statType):0.0}), 비용 {cost}");
        return true;
    }
}
