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

        // 미리 깔아둔 파일은 돈이 없으면 꺼진 상태로 시작
        if (_model.Count <= 0)
        {
            _view.Hide();
        }
    }

    // 게스트가 요금을 놓을 때 호출 — 누적하고, 꺼져 있으면 켠다
    public void Deposit(int amount)
    {
        _model.Add(amount);
        _view.Show();
        _view.UpdateVisual(_model.Count);
    }

    public void Collect(ICurrencyReceiver receiver, Transform target)
    {
        int collected = _model.Collect();
        if (collected <= 0)
        {
            return;
        }

        _view.PlayCollectEffect(target, () =>
        {
            receiver.AddMoney(collected);

            // 연출 중 게스트가 또 놓았으면 켜진 채 유지, 아니면 비활성화 후 재사용 대기
            if (_model.Count > 0)
            {
                _view.UpdateVisual(_model.Count);
            }
            else
            {
                _view.Hide();
            }
        });
    }
}
