 using System;
using System.Collections.Generic;

public class ProgressionModel
{
    private readonly HashSet<string> _unlockedIds = new HashSet<string>();

    public int Total { get; }
    public int Completed => _unlockedIds.Count;

    public event Action<int, int> OnProgressChanged;

    public ProgressionModel(int total)
    {
        Total = total;
    }

    public bool Report(string unlockId)
    {
        if (string.IsNullOrEmpty(unlockId) || !_unlockedIds.Add(unlockId))
        {
            return false;
        }

        OnProgressChanged?.Invoke(Completed, Total);
        return true;
    }

    public bool IsUnlocked(string unlockId)
    {
        return _unlockedIds.Contains(unlockId);
    }
}
