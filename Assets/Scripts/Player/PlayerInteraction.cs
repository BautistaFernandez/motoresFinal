using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Sistema moderno obligatorio para Unity 6

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Cámara y Raycast")]
    [SerializeField] private Transform cam;
    [SerializeField] private float interactionDistance = 3f;

    [Header("UI y Textos")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private bool cajaDescubierta = false;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    [Header("Crosshair")]
    [SerializeField] private CrosshairController crosshair;

    public bool TieneFusible { get; set; } = false;

    private void Update()
    {
        UpdateUI();

        // Control unificado del Input System Moderno para la tecla E
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractionTry();
        }
    }

    private void UpdateUI()
    {
        if (cam == null || promptText == null) return;

        RaycastHit hit;
        bool detectado = false;
        string textoAmostrar = "";

        // RAYCAST LIBRE: Sin máscaras que bloqueen la visión para que no se escape nada
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            // ESCÁNER TOTAL EN CASCADA: Lee el objeto y todos sus padres en la jerarquía
            Component[] componentes = hit.collider.GetComponentsInParent<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null || comp is Transform || comp is Collider || comp is MeshRenderer) continue;

                string nombre = comp.GetType().Name.ToLower();

                // 1. Cambio de casa (Prioridad)
                if (nombre.Contains("objetoinspeccionable")) { textoAmostrar = "[E] Inspeccionar"; detectado = true; break; }

                // 2. Linterna (Agregada para que la detecte sí o sí)
                if (nombre.Contains("linterna")) { textoAmostrar = "[E] Agarrar Linterna"; detectado = true; break; }

                // 3. Notas y Llaves
                if (nombre.Contains("nota")) { textoAmostrar = "[E] Leer Nota"; detectado = true; break; }
                if (nombre.Contains("llave")) { textoAmostrar = "[E] Agarrar Llave"; detectado = true; break; }

                // 4. Puertas de tu compañero
                if (nombre.Contains("controladorpuerta") || nombre.Contains("puerta")) { textoAmostrar = "[E] Interactuar con Puerta"; detectado = true; break; }

                // 5. Fusibles y Cajas de luz
                if (nombre.Contains("fusible")) { textoAmostrar = "[E] Recoger Fusible"; detectado = true; break; }
                if (nombre.Contains("caja") || nombre.Contains("luz")) { textoAmostrar = "[E] Colocar Fusible"; detectado = true; break; }

                // 6. Cerraduras, paneles y códigos
                if (nombre.Contains("cerradura") || nombre.Contains("codigo") || nombre.Contains("magnetica") || nombre.Contains("panel")) { textoAmostrar = "[E] Usar Panel"; detectado = true; break; }
            }
        }

        // CONTROL BRUTAL DE LA UI: Prende y apaga el objeto de texto para forzar a Unity a mostrarlo
        if (detectado && textoAmostrar != "")
        {
            promptText.text = textoAmostrar;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.text = "";
            promptText.gameObject.SetActive(false);
        }

        // CONTROL DEL CROSSHAIR ORIGINAL DE TU GIT
        if (crosshair != null)
        {
            // NOTA: Si en la consola te salta el error rojo "CS1061 SetInteractable no existe", 
            // simplemente borrá o comentá la línea de abajo. La dejo porque estaba en tu Git base.
            // crosshair.SetInteractable(detectado);
        }
    }

    private void InteractionTry()
    {
        if (cam == null) return;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            // A. CAMBIO A LA HOUSE EVIL (Intacto, no se rompe la intro)
            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null && InspectorManager.Instance != null)
            {
                InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                return;
            }

            // B. LA PUERTA EXACTA DE TU COMPAÑERO (Llama a IntentarAbrir)
            ControladorPuerta puerta = hit.collider.GetComponentInParent<ControladorPuerta>();
            if (puerta != null)
            {
                puerta.IntentarAbrir();
                return;
            }

            // C. DISPARADOR UNIVERSAL (Linterna, Fusibles, Cerraduras, Llaves, Notas)
            Component[] componentes = hit.collider.GetComponentsInParent<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null || comp is Transform || comp is Collider || comp is MeshRenderer) continue;

                string nombre = comp.GetType().Name.ToLower();

                // Cambio de estado si es el fusible
                if (nombre.Contains("fusible"))
                {
                    TieneFusible = true;
                }

                // Dispara todas las señales de interacción posibles de tu equipo al objeto
                comp.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Interaccion", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Recoger", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("AbrirCerradura", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}