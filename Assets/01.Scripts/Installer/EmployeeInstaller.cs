using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EmployeeView))]
public class EmployeeInstaller : MonoBehaviour
{
    [SerializeField] private EmployeeRole _role;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _guideGaugeRatePerSecond = 0.5f;

    // Stage.Awake에서 Context가 조립된 뒤에 참조해야 하므로 Start에서 조립
    private void Start()
    {
        EmployeeView view = GetComponent<EmployeeView>();
        EmployeeModel model = new EmployeeModel(_moveSpeed, _guideGaugeRatePerSecond);
        IEmployeeJob job = CreateJob();

        // 배치된 초기 위치가 곧 대기(휴식) 지점
        EmployeePresenter presenter = new EmployeePresenter(model, view, job, transform.position);
        view.Construct(presenter);
        presenter.Setup();
    }

    private IEmployeeJob CreateJob()
    {
        switch (_role)
        {
            case EmployeeRole.CounterClerk:
                return new CounterClerkJob(Stage.Instance.CounterStation.EmployeeWorkPoint);

            case EmployeeRole.Cleaner:
                EquipmentView[] equipments = Stage.Instance.EquipmentStations
                    .SelectMany(station => station.Equipments)
                    .ToArray();
                return new CleanerJob(equipments);

            default:
                Debug.LogError($"[EmployeeInstaller] 처리되지 않은 직원 역할: {_role}");
                return null;
        }
    }
}
