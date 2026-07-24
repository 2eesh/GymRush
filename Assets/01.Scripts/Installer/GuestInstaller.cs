using UnityEngine;

[RequireComponent(typeof(GuestView))]
public class GuestInstaller : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3.0f;

    private void Awake()
    {
        GuestView view = GetComponent<GuestView>();
        GuestModel model = new GuestModel(_moveSpeed);
        GuestPresenter presenter = new GuestPresenter(model, view);
        view.Construct(presenter);
    }
}
