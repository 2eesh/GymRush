using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDataView : MonoBehaviour, IPlayerDataView
{
    [SerializeField] private TextMeshProUGUI _questProgressText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Image _fillGaugeQuestProgress;
    
    public void UpdateQuestProgress(int complate, int total)
    {
        _questProgressText.text = $"{complate}/{total}";
        _fillGaugeQuestProgress.fillAmount = total > 0 ? (float)complate / total : 0f;
    }

    public void UpdateMoney(int money)
    {
        _moneyText.text = $"{money}";
    }
}
