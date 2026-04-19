using UnityEngine;
using UnityEngine.SceneManagement; 

public class LlaveVictoria : MonoBehaviour
{
    public string nombreEscenaVictoria = "WinScene";
    public GameObject textoE;
    private bool estaCerca = false;

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
        }
    }

    public void Victoria()
    {
        if (estaCerca)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(nombreEscenaVictoria);
        }
    }
}