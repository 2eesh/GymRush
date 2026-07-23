public class PlayerDataPresenter
{
    private readonly PlayerModel _model;
    private readonly ProgressionModel _progression;
    private readonly IPlayerDataView _dataView;

    public int Money => _model.Money.Amount;

    public PlayerDataPresenter(PlayerModel model, ProgressionModel progression, IPlayerDataView dataView)
    {
        _model = model;
        _progression = progression;
        _dataView = dataView;

        _progression.OnProgressChanged += _dataView.UpdateQuestProgress;

        _dataView.UpdateMoney(_model.Money.Amount);
        _dataView.UpdateQuestProgress(_progression.Completed, _progression.Total);
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
