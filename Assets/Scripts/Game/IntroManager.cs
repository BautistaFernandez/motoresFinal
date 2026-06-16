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
    [SerializeField] private Transform spawnHouseEvil;

    [Header("Componentes de la UI")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [SerializeField] private TextMeshProUGUI textoPensamiento;

    [Header("Referencias de Eventos")]
    [Tooltip("Arrastrá acá el objeto que tiene el script DollEventManager")]
    [SerializeField] private DollEventManager dollEventManager;

    [Header("Muñeca flotante (casa malvada)")]
    [SerializeField] private GameObject munecaFlotante;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float tiempoEsperaDesaparicion = 2f;

    [Header("Cámara")]
    [SerializeField] private Unity.Cinemachine.CinemachinePanTilt cinemachinePanTilt;

    [Header("Flashlight")]
    [SerializeField] private Flashlight flashlight;


    private Rigidbody playerRb;
    private Movement playerMovement;
    private bool introCompletada = false;
    private bool munecaYaVistaProgramada = false;
    private bool deteccionMunecaActiva = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (flashlight != null) flashlight.OnPickedUp += HandleLinternaRecogida;
    }

    private void OnDestroy()
    {
        if (flashlight != null) flashlight.OnPickedUp -= HandleLinternaRecogida;
    }

    private void HandleLinternaRecogida()
    {
        StartCoroutine(MostrarPensamientoLinterna());
    }

    private IEnumerator MostrarPensamientoLinterna()
    {
        if (textoPensamiento != null)
        {
            textoPensamiento.gameObject.SetActive(true);
            textoPensamiento.text = "Y este corte de luz? Tendré que usar la linterna para ver";
            yield return new WaitForSeconds(6f);
            textoPensamiento.gameObject.SetActive(false);
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

    private void Update()
    {

        if (deteccionMunecaActiva && !munecaYaVistaProgramada && munecaFlotante != null && munecaFlotante.activeSelf)
        {
            if (MuñecaVistaPorPlayer())
            {
                munecaYaVistaProgramada = true;
                StartCoroutine(DesaparecerMuñecaTrasEspera());
            }
        }
    }

    private bool MuñecaVistaPorPlayer()
    {
        if (playerCamera == null || munecaFlotante == null) return false;

        Vector3 viewportPos = playerCamera.WorldToViewportPoint(munecaFlotante.transform.position);
        return viewportPos.z > 0 &&
               viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1;
    }

    private IEnumerator DesaparecerMuñecaTrasEspera()
    {
        yield return new WaitForSeconds(tiempoEsperaDesaparicion);
        if (munecaFlotante != null) munecaFlotante.SetActive(false);
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

        if (cinemachinePanTilt != null && posicionSillon != null)
        {
            cinemachinePanTilt.PanAxis.Value = posicionSillon.eulerAngles.y - playerTransform.eulerAngles.y;
            cinemachinePanTilt.TiltAxis.Value = 0f;
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

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerRb != null) playerRb.linearVelocity = Vector3.zero;

        if (playerTransform != null && spawnHouseEvil != null)
        {
            playerTransform.position = spawnHouseEvil.position;
            playerTransform.rotation = spawnHouseEvil.rotation;
        }

        if (cinemachinePanTilt != null && spawnHouseEvil != null)
        {
            cinemachinePanTilt.PanAxis.Value = spawnHouseEvil.eulerAngles.y - playerTransform.eulerAngles.y;
            cinemachinePanTilt.TiltAxis.Value = 0f;
        }

        yield return new WaitForEndOfFrame();

        if (playerMovement != null) playerMovement.enabled = true;

        //yield return new WaitForSeconds(1.5f);
        deteccionMunecaActiva = true;

        //yield return new WaitForSeconds(4.5f);
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