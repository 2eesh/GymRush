// 업그레이드가 Multiplier만 바꾸도록 기본값과 배율을 분리한 스탯 — 기본값은 훼손되지 않는다
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
