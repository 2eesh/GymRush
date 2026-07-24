using System;
using System.Collections;
using UnityEngine;

// 시간 경과로 서비스가 끝나는 콘텐츠 — 벤치·스트레칭·트레드밀·락커 등 대부분의 스테이션이 사용
public class TimedStationContents : StationContents
{
    [SerializeField] private float _serviceDuration = 3.0f;

    public override void BeginService(ContentUnit unit, GuestPresenter guest, Action onComplete)
    {
        StartCoroutine(TimerRoutine(onComplete));
    }

    private IEnumerator TimerRoutine(Action onComplete)
    {
        yield return new WaitForSeconds(_serviceDuration);

        onComplete?.Invoke();
    }
}
