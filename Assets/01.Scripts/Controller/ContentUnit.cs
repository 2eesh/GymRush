using UnityEngine;

// 구매(해금) 단위 하나 = 슬롯 + 전용 MoneyPile + (선택) 공사 존의 자기완결 묶음.
// 이 컴포넌트가 붙는 루트는 항상 활성이어야 하고, 해금 여부는 _slotRoot의 활성 여부로 표현된다.
public class ContentUnit : MonoBehaviour
{
    [Tooltip("이 유닛의 슬롯 오브젝트(BenchPress, Locker 등). 활성 여부 = 해금 여부. 해금형 유닛은 비활성으로 시작")]
    [SerializeField] private GameObject _slotRoot;
    [Tooltip("이 유닛에서 발생한 요금이 쌓이는 전용 MoneyPile")]
    [SerializeField] private MoneyPileView _moneyPile;
    [Tooltip("이 유닛의 이용 요금. 손님이 서비스 후 이 유닛의 MoneyPile에 지불")]
    [SerializeField] private int _serviceFee;

    private ISlotViewProvider _slotProvider;

    public int ServiceFee => _serviceFee;

    public bool IsUnlocked => _slotRoot != null && _slotRoot.activeInHierarchy;

    // 해금 전(비활성)에는 Installer가 돌지 않아 Presenter 미조립 → null일 수 있음
    public IUsableSlot Slot => _slotProvider?.Slot;

    public bool CanUse => IsUnlocked && Slot != null && Slot.CanUse;

    public Vector3 MoneyPilePosition => _moneyPile != null ? _moneyPile.transform.position : transform.position;

    private void Awake()
    {
        if (_slotRoot != null)
        {
            _slotProvider = _slotRoot.GetComponentInChildren<ISlotViewProvider>(true);

            if (_slotProvider == null)
            {
                Debug.LogWarning($"[{name}] SlotRoot에서 ISlotViewProvider를 찾지 못했습니다.", this);
            }
        }
        else
        {
            Debug.LogWarning($"[{name}] SlotRoot가 연결되지 않았습니다.", this);
        }
    }

    public void DepositMoney(int amount)
    {
        if (_moneyPile == null)
        {
            Debug.LogWarning($"[{name}] MoneyPile이 연결되지 않아 요금을 적립할 수 없습니다.");
            return;
        }

        _moneyPile.Deposit(amount);
    }
}
