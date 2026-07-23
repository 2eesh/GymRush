using UnityEngine;

public class MoneyPileModel
{
    public int Count { get; private set; }

    public MoneyPileModel(int count)
    {
        Count = Mathf.Max(0, count);
    }

    public void Add(int amount)
    {
        if (amount > 0)
        {
            Count += amount;
        }
    }

    public int Collect()
    {
        int collected = Count;
        Count = 0;
        return collected;
    }
}
