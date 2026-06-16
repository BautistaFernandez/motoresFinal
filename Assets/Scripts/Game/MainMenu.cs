using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject panelControles;

    void Update()
    {
 
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (panelControles.activeSelf)
            {
                CerrarControles();
            }
        }
    }

    public void Jugar() => SceneManager.LoadScene("Casa01");
    public void AbrirControles() => panelControles.SetActive(true);
    public void CerrarControles() => panelControles.SetActive(false);
    public void Salir() => Application.Quit();
}