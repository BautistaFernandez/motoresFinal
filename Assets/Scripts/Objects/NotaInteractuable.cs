using UnityEngine;
using UnityEngine.InputSystem; 

public class NotaInteractuable : MonoBehaviour
{
    public GameObject canvasCarta; 
    public GameObject textoE;      
    private bool estaCerca = false;
    private bool cartaAbierta = false;

    void Update()
    {

        if (cartaAbierta && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CerrarCarta();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = true;
            textoE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
            textoE.SetActive(false);
            CerrarCarta(); 
        }
    }

    public void Interactuar()
    {
        if (estaCerca)
        {
            cartaAbierta = true;
            canvasCarta.SetActive(true);
            textoE.SetActive(false); 
            Time.timeScale = 0f;      
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void CerrarCarta()
    {
        cartaAbierta = false;
        canvasCarta.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (estaCerca) textoE.SetActive(true);
    }
}