using UnityEngine;

public class PlayerModel : CharacterModel
{
    public CurrencyModel Money { get; set; }

    public PlayerModel(Vector2 direction, float moveSpeed, float guideGaugeRatePerSecond, int money) : base(direction,  moveSpeed, guideGaugeRatePerSecond)
    {
        Money = new CurrencyModel();
        Money.Amount = money;
    }
}
