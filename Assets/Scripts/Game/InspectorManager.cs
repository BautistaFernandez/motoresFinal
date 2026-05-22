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

    [Header("Configuración")]
    [SerializeField] private float velocidadRotacion = 300f;

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
    }

    private void Update()
    {
        if (!inspeccionando) return;

        RotarObjeto();

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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
        objeto.transform.localPosition = Vector3.forward * objeto.GetDistanciaCamara();
        objeto.transform.localRotation = Quaternion.Euler(objeto.GetRotacionInicial());

        Vector3 escalaPadre = playerCamera.transform.lossyScale;
        objeto.transform.localScale = new Vector3(
            escalaGlobal.x / escalaPadre.x,
            escalaGlobal.y / escalaPadre.y,
            escalaGlobal.z / escalaPadre.z
        );

        if (inspectionBackground != null) inspectionBackground.SetActive(true);
        if (promptText != null) promptText.gameObject.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerInteractionScript != null) playerInteractionScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void TerminarInspeccion()
    {
        if (objetoActual == null) return;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RotarObjeto()
    {
        if (Mouse.current == null || objetoActual == null) return;

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            float rotacionY = -delta.x * velocidadRotacion * Time.deltaTime * 0.01f;
            float rotacionX = delta.y * velocidadRotacion * Time.deltaTime * 0.01f;

            objetoActual.transform.Rotate(Vector3.up, rotacionY, Space.World);
            objetoActual.transform.Rotate(Vector3.right, rotacionX, Space.World);
        }
    }
}
