using System;
using UnityEngine;

// 직원 업그레이드 패널 — 열기/닫기와 직원 카드의 생성/제거만 담당하고, 내용 구성은 Presenter가 한다
public class UIEmployeeUpgradePanelView : MonoBehaviour
{
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private Transform _entryContainer;
    [SerializeField] private UIEmployeeUpgradeEntryView _entryPrefab;

    public event Action OnOpened;
    
    // HUD의 열기 버튼 OnClick에 연결
    public void Open()
    {
        _panelRoot.SetActive(true);
        OnOpened?.Invoke();
    }

    // 패널의 닫기 버튼 OnClick에 연결
    public void Close()
    {
        _panelRoot.SetActive(false);
    }

    public void ClearEntries()
    {
        for (int i = _entryContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(_entryContainer.GetChild(i).gameObject);
        }
    }

    public UIEmployeeUpgradeEntryView AddEntry()
    {
        return Instantiate(_entryPrefab, _entryContainer);
    }
}
