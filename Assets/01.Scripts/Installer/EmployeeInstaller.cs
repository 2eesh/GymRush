using System.Collections.Generic;
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
                if (Stage.Instance.CounterStation.Contents is not GaugeStationContents counterContents)
                {
                    Debug.LogError("[EmployeeInstaller] CounterStation의 Contents가 GaugeStationContents가 아닙니다.", this);
                    return null;
                }

                Transform workPoint = counterContents.ClaimWorkPoint();
                if (workPoint == null)
                {
                    Debug.LogError("[EmployeeInstaller] 카운터에 빈 근무점이 없습니다. 카운터 유닛 수보다 직원이 많습니다.", this);
                    return null;
                }

                return new CounterClerkJob(workPoint);

            case EmployeeRole.Cleaner:
                var equipments = new List<EquipmentView>();
                foreach (StationController station in Stage.Instance.EquipmentStations)
                {
                    equipments.AddRange(station.Contents.Equipments);
                }

                return new CleanerJob(equipments.ToArray());

            default:
                Debug.LogError($"[EmployeeInstaller] 처리되지 않은 직원 역할: {_role}");
                return null;
        }
    }
}
