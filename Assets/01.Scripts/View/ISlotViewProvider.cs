// 슬롯 뷰(Locker/Counter/Equipment)가 자기 Presenter를 IUsableSlot으로 노출하기 위한 공통 창구
public interface ISlotViewProvider
{
    // Presenter가 조립되기 전(비활성 시작 등)이면 null
    IUsableSlot Slot { get; }
}
