using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    [Header("Crosshair")]
    [SerializeField] private CrosshairController crosshair;

    public bool TieneFusible { get; set; } = false;

    private void Update()
    {
        UpdateUI();

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractionTry();
        }
    }

    private void UpdateUI()
    {
        if (cam == null) return;

        if (promptText == null)
        {
            Debug.LogWarning("¡Falta asignar el Prompt Text en el Inspector del Player!");
            return;
        }

        RaycastHit hit;
        bool detectado = false;

       
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            // 1. DIBUJO COCINA (Cambio de dimensiones)
            if (hit.collider.GetComponent<ObjetoInspeccionable>() != null)
            {
                promptText.text = "[E] Inspeccionar";
                detectado = true;
            }
            // 2. NOTAS
            else if (hit.collider.GetComponent<NotaInteractuable>() != null)
            {
                promptText.text = "[E] Leer Nota";
                detectado = true;
            }
            // 3. LLAVES
            else if (hit.collider.GetComponent<LlaveRecogible>() != null)
            {
                promptText.text = "[E] Agarrar Llave";
                detectado = true;
            }
            // 4. PUERTAS DE TU COMPAÑERO (Buscamos en el padre también)
            else if (hit.collider.GetComponentInParent<ControladorPuerta>() != null)
            {
                promptText.text = "[E] Interactuar con Puerta";
                detectado = true;
            }
            // 5. FUSIBLES Y CAJAS
            else if (hit.collider.GetComponentInParent<FusibleRecogible>() != null)
            {
                promptText.text = "[E] Recoger Fusible";
                detectado = true;
            }
            else if (hit.collider.GetComponentInParent<CajaDeLuz>() != null)
            {
                promptText.text = "[E] Colocar Fusible";
                detectado = true;
            }
            // 6. CERRADURA ELÉCTRICA
            else
            {
                Component[] comps = hit.collider.GetComponentsInParent<Component>();
                foreach (var c in comps)
                {
                    if (c == null) continue;
                    string n = c.GetType().Name.ToLower();
                    if (n.Contains("cerradura") || n.Contains("codigo") || n.Contains("magnetica") || n.Contains("panel"))
                    {
                        promptText.text = "[E] Usar Panel";
                        detectado = true;
                        break;
                    }
                }
            }
        }

        if (!detectado)
        {
            promptText.text = "";
        }
    }

    private void InteractionTry()
    {
        if (cam == null) return;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            // A. CAMBIO A LA HOUSE EVIL
            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null && InspectorManager.Instance != null)
            {
                InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                return;
            }

            // B. NOTAS
            NotaInteractuable nota = hit.collider.GetComponent<NotaInteractuable>();
            if (nota != null) { nota.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver); return; }

            // C. LLAVES
            LlaveRecogible llave = hit.collider.GetComponent<LlaveRecogible>();
            if (llave != null) { llave.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver); return; }

            // D. PUERTAS (LA SOLUCIÓN MÁGICA BASADA EN TU CÓDIGO)
            ControladorPuerta puerta = hit.collider.GetComponentInParent<ControladorPuerta>();
            if (puerta != null)
            {
                // Llamamos a la función real que existe en su script
                puerta.IntentarAbrir();
                return;
            }

            // E. FUSIBLES
            FusibleRecogible fusible = hit.collider.GetComponentInParent<FusibleRecogible>();
            if (fusible != null)
            {
                TieneFusible = true;
                fusible.SendMessage("Recoger", SendMessageOptions.DontRequireReceiver);
                fusible.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver);
                return;
            }

            // F. CAJA DE LUZ
            CajaDeLuz caja = hit.collider.GetComponentInParent<CajaDeLuz>();
            if (caja != null) { caja.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver); return; }

            // G. CERRADURA ELÉCTRICA
            hit.collider.SendMessageUpwards("Interactuar", SendMessageOptions.DontRequireReceiver);
            hit.collider.SendMessageUpwards("Interaccion", SendMessageOptions.DontRequireReceiver);
        }
    }
}