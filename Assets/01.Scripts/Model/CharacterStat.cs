// 기본값(Base)과 배율(Multiplier)을 분리해 보관하는 스탯.
// 업그레이드는 Multiplier만 바꾸므로 기본값이 훼손되지 않는다.
public class CharacterStat
{
    public float BaseValue { get; set; }
    public float Multiplier { get; set; } = 1f;

    public float Value => BaseValue * Multiplier;

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
    }
}
