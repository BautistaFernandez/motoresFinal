using UnityEngine;

public class NotaInteractuable : MonoBehaviour
{
    public GameObject canvasCarta;
    private bool estaAbierta = false;

    public void Interactuar()
    {
        estaAbierta = true;
        canvasCarta.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (estaAbierta && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            estaAbierta = false;
            canvasCarta.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}