using UnityEngine;

[RequireComponent(typeof(LockerView))]
public class LockerInstaller : MonoBehaviour
{
    private void Start()
    {
        LockerView view = GetComponent<LockerView>();
        LockerModel model = new LockerModel();
        LockerPresenter presenter = new LockerPresenter(model, view);
        
        view.Construct(presenter);
    }
}
