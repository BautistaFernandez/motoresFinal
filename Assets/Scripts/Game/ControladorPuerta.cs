using UnityEngine;
using UnityEngine.InputSystem; 

public class ControladorPuerta : MonoBehaviour
{
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;

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
            estaAbierta = !estaAbierta;

            int anguloTruncado = (int)transform.localEulerAngles.y;
            Debug.Log("Puerta movida. Ángulo actual: " + anguloTruncado);
        }

        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }
}

// agregar tag del player al personaje para el trigger funcione, y agregar un collider con isTrigger al area de la puerta para detectar la cercania del jugador.
//agregar collider a la puerta y istrigger
//asigar script al controladorPuertta al objeto puerta o donde hace pivot de visagra
// boton de accion la tecla E para estas cosas ?
// Cuando caminás hacia la zona del Box Collider de la puerta, el método OnTriggerEnter se activa y pone jugadorCerca en true.