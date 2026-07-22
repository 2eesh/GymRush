using UnityEngine;

// 직원의 "무슨 일을 어디서 하는가"를 역할별로 분리하는 전략 인터페이스.
// 새 직원 역할이 필요하면 이 인터페이스 구현체만 추가하면 된다.
public interface IEmployeeJob
{
    // 지금 할 일이 있으면 작업 지점을 반환. (작업 지점 = 해당 게이지 존 내부의 위치)
    bool TryGetWork(out Vector2 workPoint);

    // 현재 맡은 작업이 끝났는가. 상주형 직무(카운터)는 항상 false.
    bool IsWorkFinished();
}
