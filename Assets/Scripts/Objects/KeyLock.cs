using System;
using UnityEngine;

public class KeyLock : MonoBehaviour, ILock
{
    [SerializeField] private bool tieneLlave = false;
    [SerializeField] private string lockMessage = "Necesitas una llave";

    public bool IsUnlocked => tieneLlave;
    public string GetLockMessage() => lockMessage;

    public void Unlock()
    {
        tieneLlave = true;
    }
}
