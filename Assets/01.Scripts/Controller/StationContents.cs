using System;
using System.Collections.Generic;
using UnityEngine;

// 스테이션의 "무엇을 파는가" — 요금과 ContentUnit 집계를 담당하는 공통 베이스.
// Contents 루트(항상 활성)에 파생 클래스를 부착한다. 서비스 종료 방식은 파생이 결정:
//   TimedStationContents — 시간 경과(타이머)로 종료
//   GaugeStationContents — 직원이 게이지를 채워야 종료
public abstract class StationContents : MonoBehaviour
{
    private ContentUnit[] _units;
    private EquipmentView[] _equipments;

    // 청소 직원(CleanerJob)이 순회할 기구 목록. 기구가 없는 스테이션(카운터/락커)은 빈 배열
    public IReadOnlyList<EquipmentView> Equipments => _equipments;

    public bool HasUnlockedUnit
    {
        get
        {
            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i].IsUnlocked)
                {
                    return true;
                }
            }

            return false;
        }
    }

    protected virtual void Awake()
    {
        _units = GetComponentsInChildren<ContentUnit>(true);
        _equipments = GetComponentsInChildren<EquipmentView>(true);

        if (_units.Length == 0)
        {
            Debug.LogWarning($"[{name}] ContentUnit이 하나도 없습니다.", this);
        }
    }

    // 사용 가능한(해금 + 비어있는) 첫 유닛의 슬롯을 점유
    public bool TryClaimUnit(out ContentUnit unit, out Vector3 usePoint)
    {
        for (int i = 0; i < _units.Length; i++)
        {
            if (_units[i].CanUse)
            {
                unit = _units[i];
                unit.Slot.StartUse();
                usePoint = unit.Slot.SeatPoint.position;
                return true;
            }
        }

        unit = null;
        usePoint = default;
        return false;
    }

    // 서비스 실행. 완료 시 onComplete 호출 — 컨트롤러는 서비스 방식(타이머/게이지)을 알 필요 없음.
    // unit은 손님에게 배정된 유닛 — 게이지처럼 유닛별 진행이 필요한 방식이 사용
    public abstract void BeginService(ContentUnit unit, GuestPresenter guest, Action onComplete);
}
