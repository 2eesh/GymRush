using UnityEngine;

public class GuestModel : CharacterModel
{
    public bool HasPaidCounter { get; set; }
    public bool HasChangedClothes { get; set; }
    public bool HasExercised { get; set; }

    public GuestModel(float moveSpeed)
        : base(Vector2.zero, moveSpeed, 0f)
    {
        Setup();
    }

    // 풀 재사용 시 한 방문 주기의 상태를 초기화
    public void Setup()
    {
        Direction = Vector2.zero;
        HasPaidCounter = false;
        HasChangedClothes = false;
        HasExercised = false;
    }
}
