using UnityEngine;

public class KeypadLock : MonoBehaviour, ILock
{
    [SerializeField] private bool codigoCorrecto = false;
    [SerializeField] private string lockMessage = "La puerta tiene un código de 3 dígitos";

    public bool IsUnlocked => codigoCorrecto;
    public string GetLockMessage() => lockMessage;

    public void Unlock()
    {
        codigoCorrecto = true;
    }
}
