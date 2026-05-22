using UnityEngine;
using UnityEngine.InputSystem;

public class InspectorManager : MonoBehaviour
{
    public static InspectorManager Instance;

    [Header("Referencias")]
    [SerializeField] private Camera inspectionCamera;
    [SerializeField] private Transform pivotInspeccion;
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private MonoBehaviour playerInteractionScript;

    [Header("Configuración")]
    [SerializeField] private float velocidadRotacion = 300f;

    private ObjetoInspeccionable objetoActual;
    private Vector3 posicionOriginalObjeto;
    private Quaternion rotacionOriginalObjeto;
    private Transform padreOriginal;
    private bool inspeccionando = false;

    private void Awake()
    {
        Instance = this;
        if (inspectionCamera != null) inspectionCamera.gameObject.SetActive(false);
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
        objetoActual = objeto;
        inspeccionando = true;

        // Guardar estado original
        posicionOriginalObjeto = objeto.transform.position;
        rotacionOriginalObjeto = objeto.transform.rotation;
        padreOriginal = objeto.transform.parent;

        // Mover objeto al pivot adelante de la cámara
        objeto.transform.SetParent(pivotInspeccion);
        objeto.transform.localPosition = Vector3.forward * objeto.GetDistanciaCamara();
        objeto.transform.localRotation = Quaternion.Euler(objeto.GetRotacionInicial());

        // Activar cámara de inspección
        if (inspectionCamera != null) inspectionCamera.gameObject.SetActive(true);

        // Bloquear movimiento del player y mostrar cursor
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerInteractionScript != null) playerInteractionScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void TerminarInspeccion()
    {
        if (objetoActual == null) return;

        // Restaurar el objeto a su lugar original
        objetoActual.transform.SetParent(padreOriginal);
        objetoActual.transform.position = posicionOriginalObjeto;
        objetoActual.transform.rotation = rotacionOriginalObjeto;

        objetoActual.TerminarInspeccion();
        objetoActual = null;
        inspeccionando = false;

        // Apagar cámara de inspección
        if (inspectionCamera != null) inspectionCamera.gameObject.SetActive(false);

        // Reactivar movimiento del player y ocultar cursor
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

            // Rotar en espacio mundial para que sea intuitivo (no se acumulan rotaciones raras)
            objetoActual.transform.Rotate(Vector3.up, rotacionY, Space.World);
            objetoActual.transform.Rotate(Vector3.right, rotacionX, Space.World);
        }
    }
}
