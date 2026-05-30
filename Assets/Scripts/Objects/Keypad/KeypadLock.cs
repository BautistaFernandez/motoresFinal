using UnityEngine;

public class KeypadLock : MonoBehaviour, ILock
{
    [SerializeField] private bool codigoCorrecto = false;
    [SerializeField] private string lockMessage = "La puerta tiene un código";

    public bool IsUnlocked => codigoCorrecto;
    public string GetLockMessage() => lockMessage;

    public void Unlock()
    {
        codigoCorrecto = true;
    }
}
