using System;
using System.Collections.Generic;

// 직원 업그레이드 데이터 — 직원 개체(Id)별로 스탯 종류마다 현재 레벨을 보관한다.
// 레벨당 +10%, 최대 5레벨(총 +50%). 비용/재화 검증은 매니저 몫이고 여기는 순수 데이터.
public class EmployeeUpgradeModel
{
    public const int MaxLevel = 5;
    public const float BonusPerLevel = 0.1f;
    public const int BaseCostPerLevel = 100;

    // 레벨 변경 시 (직원 Id, 스탯 종류, 새 레벨) 통지
    public event Action<string, EmployeeStatType, int> OnLevelChanged;

    private readonly Dictionary<(string, EmployeeStatType), int> _levels = new();

    public int GetLevel(string employeeId, EmployeeStatType statType)
    {
        return _levels.TryGetValue((employeeId, statType), out int level) ? level : 0;
    }

    public bool IsMaxLevel(string employeeId, EmployeeStatType statType)
    {
        return GetLevel(employeeId, statType) >= MaxLevel;
    }

    // 현재 레벨에 해당하는 스탯 배율 (Lv0 = 1.0, Lv5 = 1.5)
    public float GetMultiplier(string employeeId, EmployeeStatType statType)
    {
        return 1f + BonusPerLevel * GetLevel(employeeId, statType);
    }

    // 다음 레벨로 올리는 비용 (Lv0→1 = 100, Lv4→5 = 500). 만렙이면 0
    public int GetUpgradeCost(string employeeId, EmployeeStatType statType)
    {
        int level = GetLevel(employeeId, statType);
        return level >= MaxLevel ? 0 : BaseCostPerLevel * (level + 1);
    }

    public bool TryLevelUp(string employeeId, EmployeeStatType statType)
    {
        int level = GetLevel(employeeId, statType);
        if (level >= MaxLevel)
        {
            return false;
        }

        _levels[(employeeId, statType)] = level + 1;
        OnLevelChanged?.Invoke(employeeId, statType, level + 1);
        return true;
    }
}
