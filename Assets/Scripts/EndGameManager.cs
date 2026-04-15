using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public void Reintentar()
    {
        SceneManager.LoadScene("Casa01"); 
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Salir()
    {
        Application.Quit();
    }
}