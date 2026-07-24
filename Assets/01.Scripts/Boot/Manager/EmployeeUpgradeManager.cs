using System;
using UnityEngine;

// 직원 업그레이드의 단일 진입점. 레벨 데이터는 EmployeeUpgradeModel이 들고 있고,
// 여기서는 씬 접근(싱글톤)과 업그레이드 시도 절차를 담당한다.
// TODO: 5단계에서 재화(CurrencyModel) 차감 검증을 TryUpgrade에 추가
public class EmployeeUpgradeManager : SingletonMonoBehaviour<EmployeeUpgradeManager>
{
    public EmployeeUpgradeModel Upgrades { get; private set; }

    // (역할, 스탯 종류, 새 레벨) — 모델 이벤트를 그대로 중계
    public event Action<EmployeeRole, EmployeeStatType, int> OnLevelChanged;

    protected override void Awake()
    {
        base.Awake();

        Upgrades = new EmployeeUpgradeModel();
        Upgrades.OnLevelChanged += (role, statType, level) => OnLevelChanged?.Invoke(role, statType, level);
    }

    public int GetLevel(EmployeeRole role, EmployeeStatType statType)
    {
        return Upgrades.GetLevel(role, statType);
    }

    public bool IsMaxLevel(EmployeeRole role, EmployeeStatType statType)
    {
        return Upgrades.IsMaxLevel(role, statType);
    }

    public float GetMultiplier(EmployeeRole role, EmployeeStatType statType)
    {
        return Upgrades.GetMultiplier(role, statType);
    }

    public bool TryUpgrade(EmployeeRole role, EmployeeStatType statType)
    {
        if (!Upgrades.TryLevelUp(role, statType))
        {
            return false;
        }

        Debug.Log($"[EmployeeUpgrade] {role} {statType} → Lv{Upgrades.GetLevel(role, statType)} (x{Upgrades.GetMultiplier(role, statType):0.0})");
        return true;
    }
}
