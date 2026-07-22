using UnityEngine;

// 청소 직무 — Dirty 상태인 운동 기구를 찾아 그 기구의 청소 존으로 이동해 서 있는다.
// 실제 청소(게이지 충전)는 기구의 청소 게이지 존이 처리하고, 기구가 깨끗해지면 작업 완료.
public class CleanerJob : IEmployeeJob
{
    private readonly EquipmentView[] _equipments;
    private EquipmentPresenter _current;

    public CleanerJob(EquipmentView[] equipments)
    {
        _equipments = equipments;
    }

    public bool TryGetWork(out Vector2 workPoint)
    {
        workPoint = default;
        _current = null;

        foreach (EquipmentView equipment in _equipments)
        {
            if (equipment.Presenter != null && equipment.Presenter.IsDirty)
            {
                _current = equipment.Presenter;
                workPoint = equipment.CleaningPoint.position;
                return true;
            }
        }

        return false;
    }

    public bool IsWorkFinished()
    {
        // 맡은 기구가 없거나, 이미 깨끗해졌으면(플레이어가 먼저 청소한 경우 포함) 완료
        return _current == null || !_current.IsDirty;
    }
}
