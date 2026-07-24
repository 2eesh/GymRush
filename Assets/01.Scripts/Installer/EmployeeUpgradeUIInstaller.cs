using System.Collections.Generic;
using UnityEngine;

// 업그레이드 패널 조립 — 씬의 플레이어(지갑)와 매니저를 Presenter에 연결하고,
// 패널이 열릴 때마다 현재 스폰된 직원 목록을 넘겨 카드를 재구성한다
public class EmployeeUpgradeUIInstaller : MonoBehaviour
{
    [SerializeField] private UIEmployeeUpgradePanelView _panel;

    private EmployeeUpgradePresenter _presenter;

    private void Start()
    {
        PlayerView player = FindFirstObjectByType<PlayerView>();
        if (player == null)
        {
            Debug.LogError("[EmployeeUpgradeUIInstaller] 씬에서 PlayerView를 찾을 수 없습니다.", this);
            return;
        }

        _presenter = new EmployeeUpgradePresenter(EmployeeUpgradeManager.Instance, player, _panel);
        _panel.OnOpened += HandlePanelOpened;
    }

    private void OnDestroy()
    {
        _panel.OnOpened -= HandlePanelOpened;
        _presenter?.Dispose();
    }

    private void HandlePanelOpened()
    {
        var employees = new List<EmployeeModel>();
        foreach (EmployeeView view in FindObjectsByType<EmployeeView>(FindObjectsSortMode.None))
        {
            // 아직 조립 전(Presenter 없음)인 직원은 제외
            if (view.Presenter != null)
            {
                employees.Add(view.Presenter.Model);
            }
        }

        // 매번 순서가 흔들리지 않도록 Id 기준 정렬
        employees.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
        _presenter.Rebuild(employees);
    }
}
