using System.Collections.Generic;
using UnityEngine;

public class UnlockChainManager : SingletonMonoBehaviour<UnlockChainManager>
{
    [SerializeField] private UnlockChainData _chainData;

    private readonly Dictionary<string, ConstructionInteractionZoneController> _zones = new();

    public ProgressionModel Progression { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Progression = new ProgressionModel(_chainData != null ? _chainData.Entries.Length : 0);

        CollectZones();
        ValidateData();

        foreach (var zone in _zones.Values)
        {
            zone.ZoneRoot.SetActive(false);
        }

        EvaluateUnlocks();
    }

    public void ReportComplete(string unlockId)
    {
        if (!Progression.Report(unlockId))
        {
            return;
        }

        EvaluateUnlocks();
    }

    private void EvaluateUnlocks()
    {
        if (_chainData == null)
        {
            return;
        }

        foreach (var entry in _chainData.Entries)
        {
            if (Progression.IsUnlocked(entry.Id))
            {
                continue;
            }

            if (!_zones.TryGetValue(entry.Id, out var zone) || zone.ZoneRoot.activeSelf)
            {
                continue;
            }

            if (AreRequirementsMet(entry))
            {
                zone.ZoneRoot.SetActive(true);
            }
        }
    }

    private bool AreRequirementsMet(UnlockChainData.Entry entry)
    {
        if (entry.RequiredIds == null)
        {
            return true;
        }

        foreach (var requiredId in entry.RequiredIds)
        {
            if (!Progression.IsUnlocked(requiredId))
            {
                return false;
            }
        }

        return true;
    }

    private void CollectZones()
    {
        ConstructionInteractionZoneController[] zones = FindObjectsByType<ConstructionInteractionZoneController>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var zone in zones)
        {
            if (string.IsNullOrEmpty(zone.UnlockId))
            {
                Debug.LogWarning($"[UnlockChain] {zone.name}의 UnlockId가 비어있습니다.", zone);
                continue;
            }

            if (_zones.ContainsKey(zone.UnlockId))
            {
                Debug.LogWarning($"[UnlockChain] UnlockId 중복: '{zone.UnlockId}' ({zone.name})", zone);
                continue;
            }

            _zones.Add(zone.UnlockId, zone);
        }
    }

    private void ValidateData()
    {
        if (_chainData == null)
        {
            Debug.LogError("[UnlockChain] UnlockChainData가 연결되지 않았습니다.", this);
            return;
        }

        var dataIds = new HashSet<string>();
        foreach (var entry in _chainData.Entries)
        {
            if (string.IsNullOrEmpty(entry.Id))
            {
                Debug.LogWarning("[UnlockChain] 데이터에 빈 Id 항목이 있습니다.", _chainData);
                continue;
            }

            dataIds.Add(entry.Id);

            if (!_zones.ContainsKey(entry.Id))
            {
                Debug.LogWarning($"[UnlockChain] 데이터의 '{entry.Id}'에 해당하는 존이 씬에 없습니다.", _chainData);
            }

            if (entry.RequiredIds == null)
            {
                continue;
            }

            foreach (var requiredId in entry.RequiredIds)
            {
                if (System.Array.FindIndex(_chainData.Entries, e => e.Id == requiredId) < 0)
                {
                    Debug.LogWarning($"[UnlockChain] '{entry.Id}'의 선행조건 '{requiredId}'가 데이터에 없습니다.", _chainData);
                }
            }
        }

        foreach (var zoneId in _zones.Keys)
        {
            if (!dataIds.Contains(zoneId))
            {
                Debug.LogWarning($"[UnlockChain] 씬의 존 '{zoneId}'가 데이터에 정의되지 않았습니다. 해당 존은 열리지 않습니다.", _zones[zoneId]);
            }
        }
    }
}
