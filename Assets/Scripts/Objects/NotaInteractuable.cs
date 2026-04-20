using UnityEngine;

public class NotaInteractuable : MonoBehaviour
{
    public GameObject canvasCarta;
    public GameObject barreraInvisible;

    private bool estaAbierta = false;

    public void Interactuar()
    {
        estaAbierta = true;
        canvasCarta.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- Y ESTA LÓGICA ---
        if (barreraInvisible != null)
        {
            barreraInvisible.SetActive(false);
            Debug.Log("Nota leída: Barrera desactivada.");
        }
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