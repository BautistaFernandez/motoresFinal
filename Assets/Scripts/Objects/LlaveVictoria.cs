using UnityEngine;
using UnityEngine.SceneManagement;

public class LlaveVictoria : MonoBehaviour
{
    public string nombreEscenaVictoria = "WinScene";

    public void Victoria()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(nombreEscenaVictoria);
    }
}