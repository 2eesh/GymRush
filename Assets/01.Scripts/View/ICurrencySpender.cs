public interface ICurrencySpender
{
    int Amount { get; }
    void SpendMoney(int amount);
}
