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
    }

    [SerializeField] private Entry[] _entries;

    public Entry[] Entries => _entries;
}
