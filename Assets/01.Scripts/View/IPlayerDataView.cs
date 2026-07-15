using UnityEngine;

public interface IPlayerDataView
{
    void UpdateMoney(int money);
    void UpdateQuestProgress(int complete, int total);
}
