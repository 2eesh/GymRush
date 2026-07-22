using UnityEngine;

// 카운터 상주 직무 — 항상 카운터 근무 지점이 일터이고, 작업이 끝나지 않으므로 계속 서 있는다.
// 게스트 응대(게이지 충전)는 카운터의 서비스 게이지 존이 처리한다.
public class CounterClerkJob : IEmployeeJob
{
    private readonly Transform _workPoint;

    public CounterClerkJob(Transform workPoint)
    {
        _workPoint = workPoint;
    }

    public bool TryGetWork(out Vector2 workPoint)
    {
        workPoint = _workPoint.position;
        return true;
    }

    public bool IsWorkFinished()
    {
        return false;
    }
}
