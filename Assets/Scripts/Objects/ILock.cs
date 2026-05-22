using UnityEngine;

public interface ILock
{
    bool IsUnlocked { get; }
    string GetLockMessage();
}
