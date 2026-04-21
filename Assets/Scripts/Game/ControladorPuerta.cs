using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;

    [Header("UI")]
    [SerializeField] private GameObject pressEText;

    [Header("Estado")]
    public bool tieneLlave = false;
    private bool estaAbierta = false;
    private bool jugadorCerca = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, 0, anguloApertura);
    }

    void Update()
    {
        if (jugadorCerca && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            IntentarAbrir();
        }

        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    private void IntentarAbrir()
    {
       
        if (CompareTag("FinalDoor"))
        {
            if (tieneLlave) Accionar();
            else Debug.Log("Falta la llave del Garage");
        }
        
        else if (CompareTag("FinalDoor2"))
        {
            if (tieneLlave)
            {
                Accionar();
               
                if (estaAbierta) Invoke("IrAVictoria", 1.5f);
            }
            else
            {
                Debug.Log("Falta la llave de salida final");
            }
        }
       
        else
        {
            Accionar();
        }
    }

    private void Accionar()
    {
        estaAbierta = !estaAbierta;
        if (audioSource != null) audioSource.PlayOneShot(estaAbierta ? openDoor : closeDoor);
    }

    private void IrAVictoria()
    {
    
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f; 

        SceneManager.LoadScene("WinScene");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (pressEText != null) pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (pressEText != null) pressEText.SetActive(false);
        }
    }
}