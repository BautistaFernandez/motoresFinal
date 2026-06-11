using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectorManager : MonoBehaviour
{
    public static InspectorManager Instance;

    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Movement playerMovementScript;
    [SerializeField] private PlayerInteraction playerInteractionScript;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject inspectionBackground;
    [SerializeField] private GameObject inspectionHint;
    [SerializeField] private Unity.Cinemachine.CinemachineInputAxisController cinemachineInput;

    [Header("Configuración")]
    [SerializeField] private float velocidadRotacion = 300f;

    [Header("Iluminación")]
    [SerializeField] private Light inspectionLight;

    private ObjetoInspeccionable objetoActual;
    private Vector3 posicionOriginalObjeto;
    private Quaternion rotacionOriginalObjeto;
    private Vector3 escalaOriginalObjeto;
    private Transform padreOriginal;
    private bool inspeccionando = false;

    private void Awake()
    {
        Instance = this;
        if (inspectionBackground != null) inspectionBackground.SetActive(false);
        if (inspectionLight != null) inspectionLight.enabled = false;
    }

    private void Update()
    {
        if (!inspeccionando) return;

        RotarObjeto();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TerminarInspeccion();
        }
    }

    public void IniciarInspeccion(ObjetoInspeccionable objeto)
    {
        if (inspeccionando) return;

        objetoActual = objeto;
        inspeccionando = true;

        posicionOriginalObjeto = objeto.transform.position;
        rotacionOriginalObjeto = objeto.transform.rotation;
        escalaOriginalObjeto = objeto.transform.localScale;
        padreOriginal = objeto.transform.parent;

        Vector3 escalaGlobal = objeto.transform.lossyScale;

        objeto.transform.SetParent(playerCamera.transform, false);
        objeto.transform.localRotation = Quaternion.Euler(objeto.GetRotacionInicial());
        objeto.transform.localPosition = Vector3.forward * objeto.GetDistanciaCamara();

        Vector3 escalaPadre = playerCamera.transform.lossyScale;
        objeto.transform.localScale = new Vector3(
            escalaGlobal.x / escalaPadre.x,
            escalaGlobal.y / escalaPadre.y,
            escalaGlobal.z / escalaPadre.z
        );

        Physics.SyncTransforms();

        Renderer[] renderers = objeto.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds totalBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                totalBounds.Encapsulate(renderers[i].bounds);
            }

            Vector3 centroVisualMundo = totalBounds.center;
            Vector3 posicionDeseadaMundo = playerCamera.transform.position + playerCamera.transform.forward * objeto.GetDistanciaCamara();
            Vector3 offsetMundo = posicionDeseadaMundo - centroVisualMundo;
            objeto.transform.position += offsetMundo;
        }

        if (inspectionBackground != null) inspectionBackground.SetActive(true);
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (inspectionLight != null) inspectionLight.enabled = true;
        if (inspectionHint != null) inspectionHint.SetActive(true);

        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerInteractionScript != null) playerInteractionScript.enabled = false;
        if (cinemachineInput != null) cinemachineInput.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        objetoActual.IniciarInspeccion();
    }

    public void TerminarInspeccion()
    {
        if (objetoActual == null) return;

        DibujoDisparadorIntro disparadorSusto = objetoActual.GetComponent<DibujoDisparadorIntro>();
        if (disparadorSusto != null)
        {
            disparadorSusto.ActivarScreamerDeIntro();
        }

        ResetearEstadoObjeto();
    }

    public void TerminarInspeccionExterior()
    {
        if (objetoActual == null) return;
        ResetearEstadoObjeto();
    }

    private void ResetearEstadoObjeto()
    {
        objetoActual.transform.SetParent(padreOriginal);
        objetoActual.transform.position = posicionOriginalObjeto;
        objetoActual.transform.rotation = rotacionOriginalObjeto;
        objetoActual.transform.localScale = escalaOriginalObjeto;

        objetoActual.TerminarInspeccion();
        objetoActual = null;
        inspeccionando = false;

        if (inspectionBackground != null) inspectionBackground.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (playerInteractionScript != null) playerInteractionScript.enabled = true;
        if (cinemachineInput != null) cinemachineInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (inspectionLight != null) inspectionLight.enabled = false;
        if (inspectionHint != null) inspectionHint.SetActive(false);
    }

    private void RotarObjeto()
    {
        if (Mouse.current == null || objetoActual == null) return;

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            float rotacionY = -delta.x * velocidadRotacion * Time.deltaTime * 0.01f;
            float rotacionX = -delta.y * velocidadRotacion * Time.deltaTime * 0.01f;

            objetoActual.transform.Rotate(playerCamera.transform.up, rotacionY, Space.World);
            objetoActual.transform.Rotate(playerCamera.transform.right, rotacionX, Space.World);
        }
    }
}