using UnityEngine;
using System.Collections;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Instance;

    [Header("Estructuras de las Casas (Padres)")]
    [SerializeField] private GameObject houseGood;
    [SerializeField] private GameObject houseEvil;

    [Header("Puntos de Spawn del Jugador")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform posicionSillon;

    [Header("Componentes de la UI")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [SerializeField] private TextMeshProUGUI textoPensamiento;

    [Header("Referencias de Eventos")]
    [Tooltip("Arrastrá acá el objeto que tiene el script DollEventManager")]
    [SerializeField] private DollEventManager dollEventManager;

    private Rigidbody playerRb;
    private Movement playerMovement;
    private bool introCompletada = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Forzamos el estado inicial rústico y seguro
        if (houseGood != null) houseGood.SetActive(true);
        if (houseEvil != null) houseEvil.SetActive(false);

        if (playerTransform != null)
        {
            playerRb = playerTransform.GetComponent<Rigidbody>();
            playerMovement = playerTransform.GetComponent<Movement>();
        }

        StartCoroutine(RutinaInicialSegura());
    }

    private IEnumerator RutinaInicialSegura()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerRb != null) playerRb.linearVelocity = Vector3.zero;

        yield return new WaitForEndOfFrame();

        if (playerTransform != null && posicionSillon != null)
        {
            playerTransform.position = posicionSillon.position;
            playerTransform.rotation = posicionSillon.rotation;
        }

        yield return new WaitForEndOfFrame();

        if (playerMovement != null) playerMovement.enabled = true;

        if (textoPensamiento != null)
        {
            textoPensamiento.gameObject.SetActive(true);
            textoPensamiento.text = "Parece que me quedé dormido. Voy a la cocina para ver que me cocino";
            yield return new WaitForSeconds(6f);
            textoPensamiento.gameObject.SetActive(false);
        }
    }

    // El método definitivo que conecta con el Escape del dibujo
    public void DibujoDeCocinaInspeccionado()
    {
        if (introCompletada) return;
        introCompletada = true;

        StartCoroutine(RutinaScreamerYTraspaso());
    }

    private IEnumerator RutinaScreamerYTraspaso()
    {
        Debug.Log("Ejecutando secuencia de traspaso...");

        // A. ESPACIO DEL SCREAMER (Tiempo de espera)
        yield return new WaitForSecondsRealtime(2.5f);

        // B. EL CAMBIO DE DIMENSIONES DIRECTO (Esto no puede fallar jamás)
        if (houseGood != null) houseGood.SetActive(false);
        if (houseEvil != null) houseEvil.SetActive(true);

        yield return new WaitForEndOfFrame();

        // C. Frase de confusión en la UI
        if (textoPensamiento != null)
        {
            textoPensamiento.gameObject.SetActive(true);
            textoPensamiento.text = "Que está pasando acá? esta no es mi casa? necesito irme. Dejé una llave en el garage";
        }

        // D. Activamos el panel de objetivos real
        if (objectivePanel != null)
        {
            objectivePanel.Show("Busca la llave en el garage");
        }

        // E. Inicialización del contador blindada contra nulos
        if (dollEventManager != null)
        {
            dollEventManager.GatillarInicioRealDelJuego();
        }
        else
        {
            Debug.LogWarning("Nota: No asignaste el DollEventManager en el IntroManager. El juego cambia de casa igual.");
        }

        yield return new WaitForSeconds(6f);
        if (textoPensamiento != null) textoPensamiento.gameObject.SetActive(false);
    }
}