using System;
using System.Collections.Generic;

// 직원 업그레이드 데이터 — 역할(Role)별로 스탯 종류마다 현재 레벨을 보관한다.
// 레벨당 +10%, 최대 5레벨(총 +50%). 비용/재화 검증은 매니저 몫이고 여기는 순수 데이터.
public class EmployeeUpgradeModel
{
    public const int MaxLevel = 5;
    public const float BonusPerLevel = 0.1f;

    // 레벨 변경 시 (역할, 스탯 종류, 새 레벨) 통지
    public event Action<EmployeeRole, EmployeeStatType, int> OnLevelChanged;

    private readonly Dictionary<(EmployeeRole, EmployeeStatType), int> _levels = new();

    public int GetLevel(EmployeeRole role, EmployeeStatType statType)
    {
        return _levels.TryGetValue((role, statType), out int level) ? level : 0;
    }

    public bool IsMaxLevel(EmployeeRole role, EmployeeStatType statType)
    {
        return GetLevel(role, statType) >= MaxLevel;
    }

    // 현재 레벨에 해당하는 스탯 배율 (Lv0 = 1.0, Lv5 = 1.5)
    public float GetMultiplier(EmployeeRole role, EmployeeStatType statType)
    {
        return 1f + BonusPerLevel * GetLevel(role, statType);
    }

    public bool TryLevelUp(EmployeeRole role, EmployeeStatType statType)
    {
        int level = GetLevel(role, statType);
        if (level >= MaxLevel)
        {
            return false;
        }

        _levels[(role, statType)] = level + 1;
        OnLevelChanged?.Invoke(role, statType, level + 1);
        return true;
    }
}
