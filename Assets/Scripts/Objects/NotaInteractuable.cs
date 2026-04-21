using UnityEngine;
using UnityEngine.InputSystem;

public class NotaInteractuable : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvasCarta;

    private bool estaAbierta = false;

    public void Interactuar()
    {
        if (canvasCarta != null)
        {
            estaAbierta = true;
            canvasCarta.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var c in colliders) c.enabled = false;


            if (GetComponentInChildren<MeshRenderer>())
                GetComponentInChildren<MeshRenderer>().enabled = false;

            Debug.Log("Nota procesada. Camino libre para la llave.");
        }
    }

    void Update()
    {
        if (estaAbierta && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CerrarNota();
        }
    }

    public void CerrarNota()
    {
        estaAbierta = false;
        if (canvasCarta != null) canvasCarta.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Destroy(gameObject, 0.1f);
    }
}