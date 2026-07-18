using UnityEngine;

[RequireComponent(typeof(MoneyPileView))]
public class MoneyPileInstaller : MonoBehaviour
{
    [SerializeField] private int _startCount = 1;

    private void Start()
    {
        MoneyPileView view = GetComponent<MoneyPileView>();

        MoneyPileModel model = new MoneyPileModel(_startCount);
        MoneyPilePresenter presenter = new MoneyPilePresenter(model, view);

        view.Construct(presenter);
    }
}
