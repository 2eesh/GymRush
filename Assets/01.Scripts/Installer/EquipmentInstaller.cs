using UnityEngine;

[RequireComponent(typeof(EquipmentView))]
public class EquipmentInstaller : MonoBehaviour
{
    [SerializeField] private int _maxDurability = 5;

    private void Start()
    {
        EquipmentView view = GetComponent<EquipmentView>();
        EquipmentModel model = new EquipmentModel(_maxDurability);
        EquipmentPresenter presenter = new EquipmentPresenter(model, view);
        view.Construct(presenter);
    }
}
