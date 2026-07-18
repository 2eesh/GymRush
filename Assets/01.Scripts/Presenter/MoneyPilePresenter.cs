using UnityEngine;

public class MoneyPilePresenter
{
    private readonly MoneyPileModel _model;
    private readonly IMoneyPileView _view;

    public MoneyPilePresenter(MoneyPileModel model, IMoneyPileView view)
    {
        _model = model;
        _view = view;

        _view.UpdateVisual(_model.Count);
    }

    public void Collect(ICurrencyReceiver receiver, Transform target)
    {
        int collected = _model.Collect();
        if (collected <= 0)
        {
            return;
        }

        _view.PlayCollectEffect(target, () => receiver.AddMoney(collected));
    }
}
