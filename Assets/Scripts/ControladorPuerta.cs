using UnityEngine;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;     

    [Header("Interacción")]
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;
    [SerializeField] private string tagDelJugador = "Player"; 

    private bool estaAbierta = false;
    private bool jugadorCerca = false;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, anguloApertura, 0);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaInteraccion))
        {
            estaAbierta = !estaAbierta; 

            float anguloActual = transform.localEulerAngles.y;
            Debug.Log("Estado cambiado. Ángulo actual (truncado): " + (int)anguloActual);
        }

        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    // --- DETECCIÓN POR TRIGGER ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagDelJugador))
        {
            jugadorCerca = true;
            Debug.Log("Cerca de la puerta. Presioná " + teclaInteraccion);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagDelJugador))
        {
            jugadorCerca = false;
        }
    }
}

// agregar tag del player al personaje para el trigger funcione, y agregar un collider con isTrigger al area de la puerta para detectar la cercania del jugador.
//agregar collider a la puerta y istrigger
//asigar script al controladorPuertta al objeto puerta o donde hace pivot de visagra
// boton de accion la tecla E para estas cosas ?
// Cuando caminás hacia la zona del Box Collider de la puerta, el método OnTriggerEnter se activa y pone jugadorCerca en true.