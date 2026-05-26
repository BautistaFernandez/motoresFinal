using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeypadFailHandler : MonoBehaviour
{
    [SerializeField] private int maxAttempts = 3;
    [SerializeField] private GameObject screamerObject;   
    [SerializeField] private AudioSource screamerAudio;   
    [SerializeField] private float screamerDuration = 2f;
    [SerializeField] private string loseSceneName = "Lose";

    private int failCount = 0;
    private bool triggered = false;

    // Conectar al UnityEvent OnAccessDenied del Keypad desde el Inspector.
    public void OnAccessDenied()
    {
        if (triggered) return;

        failCount++;

        if (failCount >= maxAttempts)
            StartCoroutine(TriggerScreamer());
    }

    private IEnumerator TriggerScreamer()
    {
        triggered = true;

        // Destrabar cursor y pausar controles durante el screamer
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (screamerObject != null) screamerObject.SetActive(true);
        if (screamerAudio != null) screamerAudio.Play();

        yield return new WaitForSeconds(screamerDuration);

        SceneManager.LoadScene(loseSceneName);
    }

    // Para resetear si el player se escapa a un nuevo loop o reinicia.
    public void ResetFails()
    {
        failCount = 0;
        triggered = false;
        if (screamerObject != null) screamerObject.SetActive(false);
    }
}
