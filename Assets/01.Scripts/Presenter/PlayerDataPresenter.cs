public class PlayerDataPresenter
{
    private readonly PlayerModel _model;
    private readonly IPlayerDataView _dataView;

    public int Money => _model.Money.Amount;

    public PlayerDataPresenter(PlayerModel model, IPlayerDataView dataView)
    {
        _model = model;
        _dataView = dataView;

        _dataView.UpdateMoney(_model.Money.Amount);
    }

    public void AddMoney(int amount)
    {
        _model.Money.Amount += amount;
        _dataView.UpdateMoney(_model.Money.Amount);
    }

    public void SpendMoney(int amount)
    {
        _model.Money.Amount -= amount;
        _dataView.UpdateMoney(_model.Money.Amount);
    }
}
