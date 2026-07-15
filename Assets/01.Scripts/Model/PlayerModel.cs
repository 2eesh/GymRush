using UnityEngine;

public class PlayerModel : CharacterModel
{
    public CurrencyModel Money { get; set; }

    public PlayerModel(Vector2 direction, float moveSpeed, int money) : base(direction,  moveSpeed)
    {
        Money = new CurrencyModel();
        Money.Amount = money;
    }
}
