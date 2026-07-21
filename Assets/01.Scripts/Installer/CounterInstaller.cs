using UnityEngine;

[RequireComponent(typeof(CounterView))]
public class CounterInstaller : MonoBehaviour
{
    private void Start()
    {
        CounterView view = GetComponent<CounterView>();
        CounterModel model = new CounterModel();
        CounterPresenter presenter = new CounterPresenter(model, view);
        
        view.Construct(presenter);
    }
}
