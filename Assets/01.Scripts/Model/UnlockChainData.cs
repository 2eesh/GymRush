using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockChainData", menuName = "GymRush/Unlock Chain Data")]
public class UnlockChainData : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string Id;
        [Tooltip("비어있으면 게임 시작부터 열려있는 해금")]
        public string[] RequiredIds;
        [Tooltip("공사 비용. 시작 시 해당 존의 ConstructionGauge에 주입됨")]
        public int Cost;
    }

    [SerializeField] private Entry[] _entries;

    public Entry[] Entries => _entries;
}
