using System.Collections.Generic;
using UnityEngine;

// 씬에 배치된 Stage들의 레지스트리. Stage가 Awake에서 자기등록한다.
public class StageManager : SingletonMonoBehaviour<StageManager>
{
    private readonly Dictionary<int, Stage> _stages = new Dictionary<int, Stage>();

    public IReadOnlyCollection<Stage> Stages => _stages.Values;

    public void Register(Stage stage)
    {
        if (_stages.TryGetValue(stage.StageId, out Stage existing) && existing != stage)
        {
            Debug.LogError($"[StageManager] StageId 중복: {stage.StageId} ({existing.name} / {stage.name})", stage);
            return;
        }

        _stages[stage.StageId] = stage;
    }

    public void Unregister(Stage stage)
    {
        if (_stages.TryGetValue(stage.StageId, out Stage existing) && existing == stage)
        {
            _stages.Remove(stage.StageId);
        }
    }

    public Stage Get(int stageId)
    {
        if (_stages.TryGetValue(stageId, out Stage stage))
        {
            return stage;
        }

        Debug.LogError($"[StageManager] StageId {stageId}에 해당하는 Stage가 없습니다.");
        return null;
    }
}
