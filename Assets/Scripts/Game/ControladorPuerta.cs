using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorPuerta : MonoBehaviour
{
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;

    [Header("Doors Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;

    [Header("UI")]
    [SerializeField] private GameObject pressEText;

    [Header("Llave (solo aplica si la puerta tiene tag FinalDoor)")]
    public bool tieneLlave = false;

    private bool estaAbierta = false;
    private bool jugadorCerca = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, 0, anguloApertura);

        //if (pressEText != null) pressEText.SetActive(false);
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
        if (!CompareTag("FinalDoor"))
        {
            Accionar();
        }
        else if (tieneLlave)
        {
            Accionar();
        }
        // else: puerta final sin llave, no hace nada
    }

    private void Accionar()
    {
        estaAbierta = !estaAbierta;
        audioSource.PlayOneShot(estaAbierta ? openDoor : closeDoor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            Debug.Log("Enter - pressEText es null? " + (pressEText == null));
            if (pressEText != null)
            {
                pressEText.SetActive(true);
                Debug.Log("activeSelf: " + pressEText.activeSelf + " | activeInHierarchy: " + pressEText.activeInHierarchy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit trigger de: " + gameObject.name);
            jugadorCerca = false;
            if (pressEText != null) pressEText.SetActive(false);
        }
    }
}

// agregar tag del player al personaje para el trigger funcione, y agregar un collider con isTrigger al area de la puerta para detectar la cercania del jugador.
//agregar collider a la puerta y istrigger
//asigar script al controladorPuertta al objeto puerta o donde hace pivot de visagra
// boton de accion la tecla E para estas cosas ?
// Cuando caminás hacia la zona del Box Collider de la puerta, el método OnTriggerEnter se activa y pone jugadorCerca en true.