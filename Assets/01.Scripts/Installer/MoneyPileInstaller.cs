using UnityEngine;

[RequireComponent(typeof(MoneyPileView))]
public class MoneyPileInstaller : MonoBehaviour
{
    [SerializeField] private int _startCount = 1;

    private bool _installed;

    private void Start()
    {
        if (!_installed)
        {
            Setup(_startCount);
        }
    }

    // 동적 스폰 시 외부에서 금액을 지정해 조립 (게스트 지불 등)
    public void Setup(int startCount)
    {
        _installed = true;

        MoneyPileView view = GetComponent<MoneyPileView>();

        MoneyPileModel model = new MoneyPileModel(startCount);
        MoneyPilePresenter presenter = new MoneyPilePresenter(model, view);

        view.Construct(presenter);
    }
}
