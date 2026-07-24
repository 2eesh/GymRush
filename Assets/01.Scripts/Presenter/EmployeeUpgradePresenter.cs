using System.Collections.Generic;

// 업그레이드 패널의 화면 논리 — 직원 목록을 카드로 구성하고, 버튼 클릭을 구매 절차(비용 검증 포함)로 잇는다
public class EmployeeUpgradePresenter
{
    private readonly EmployeeUpgradeManager _manager;
    private readonly ICurrencySpender _spender;
    private readonly UIEmployeeUpgradePanelView _panel;
    private readonly Dictionary<string, UIEmployeeUpgradeEntryView> _entries = new();

    public EmployeeUpgradePresenter(EmployeeUpgradeManager manager, ICurrencySpender spender, UIEmployeeUpgradePanelView panel)
    {
        _manager = manager;
        _spender = spender;
        _panel = panel;

        _manager.OnLevelChanged += HandleLevelChanged;
    }

    public void Dispose()
    {
        _manager.OnLevelChanged -= HandleLevelChanged;
    }

    public void Rebuild(IReadOnlyList<EmployeeModel> employees)
    {
        _panel.ClearEntries();
        _entries.Clear();

        foreach (EmployeeModel employee in employees)
        {
            UIEmployeeUpgradeEntryView entry = _panel.AddEntry();
            string employeeId = employee.Id;

            entry.SetName(employeeId);
            entry.OnUpgradeClicked += statType => HandleUpgradeClicked(employeeId, statType);

            _entries[employeeId] = entry;
            RefreshEntry(employeeId);
        }
    }

    private void HandleUpgradeClicked(string employeeId, EmployeeStatType statType)
    {
        _manager.TryUpgrade(employeeId, statType, _spender);

        // 잔액이 변하면 다른 카드의 구매 가능 여부도 달라지므로 전체 갱신
        RefreshAll();
    }

    private void HandleLevelChanged(string employeeId, EmployeeStatType statType, int level)
    {
        if (_entries.ContainsKey(employeeId))
        {
            RefreshEntry(employeeId);
        }
    }

    private void RefreshAll()
    {
        foreach (string employeeId in _entries.Keys)
        {
            RefreshEntry(employeeId);
        }
    }

    private void RefreshEntry(string employeeId)
    {
        UIEmployeeUpgradeEntryView entry = _entries[employeeId];

        RefreshStat(entry, employeeId, EmployeeStatType.Speed);
        RefreshStat(entry, employeeId, EmployeeStatType.WorkRate);
    }

    private void RefreshStat(UIEmployeeUpgradeEntryView entry, string employeeId, EmployeeStatType statType)
    {
        int level = _manager.GetLevel(employeeId, statType);
        int cost = _manager.GetUpgradeCost(employeeId, statType);
        entry.SetStat(statType, level, EmployeeUpgradeModel.MaxLevel, cost, _spender.Amount >= cost);
    }
}
