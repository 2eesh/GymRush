using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EmployeeView))]
public class EmployeeInstaller : MonoBehaviour
{
    [SerializeField] private EmployeeRole _role;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _guideGaugeRatePerSecond = 0.5f;

    [Tooltip("소속 스테이지. 비워두면 부모에서 자동 탐색")]
    [SerializeField] private Stage _stage;

    // Stage.Awake에서 Context가 조립된 뒤에 참조해야 하므로 Start에서 조립
    private void Start()
    {
        if (_stage == null)
        {
            _stage = GetComponentInParent<Stage>();
        }

        if (_stage == null)
        {
            Debug.LogError("[EmployeeInstaller] 소속 Stage를 찾을 수 없습니다. 인스펙터에서 할당하거나 Stage 하위에 배치하세요.", this);
            return;
        }

        EmployeeView view = GetComponent<EmployeeView>();
        EmployeeModel model = new EmployeeModel(_moveSpeed, _guideGaugeRatePerSecond);
        IEmployeeJob job = CreateJob();
        if (job == null)
        {
            // 근무점 배정 실패 등으로 Job이 없으면 Tick을 중단 (에러 로그는 CreateJob이 출력)
            view.enabled = false;
            return;
        }

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
                if (_stage.CounterStation.Contents is not GaugeStationContents counterContents)
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
                foreach (StationController station in _stage.EquipmentStations)
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
