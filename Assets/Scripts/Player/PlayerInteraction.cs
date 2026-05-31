using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

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

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            Component[] componentes = hit.collider.GetComponentsInParent<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null || comp is Transform || comp is Collider || comp is MeshRenderer) continue;

                string nombre = comp.GetType().Name.ToLower();

                
                if (nombre.Contains("objetoinspeccionable")) { textoAmostrar = "[E] Inspeccionar"; detectado = true; break; }

               
                if (nombre.Contains("linterna")) { textoAmostrar = "[E] Agarrar Linterna"; detectado = true; break; }
                if (nombre.Contains("pista")) { textoAmostrar = "[E] Agarrar Pista"; detectado = true; break; }

               
                if (nombre.Contains("nota")) { textoAmostrar = "[E] Leer Nota"; detectado = true; break; }
                if (nombre.Contains("llave")) { textoAmostrar = "[E] Agarrar Llave"; detectado = true; break; }

               
                if (nombre.Contains("controladorpuerta") || nombre.Contains("puerta")) { textoAmostrar = "[E] Interactuar con Puerta"; detectado = true; break; }

               
                if (nombre.Contains("fusible")) { textoAmostrar = "[E] Recoger Fusible"; detectado = true; break; }

               
                if (nombre.Contains("caja") || nombre.Contains("luz"))
                {
                    CajaDeLuz caja = hit.collider.GetComponentInParent<CajaDeLuz>();
                    if (caja != null && caja.YaSeActivo) break; 

                    textoAmostrar = TieneFusible ? "[E] Colocar Fusible" : "Necesitas un Fusible";
                    detectado = true;
                    break;
                }

                
                if (nombre.Contains("cerradura") || nombre.Contains("codigo") || nombre.Contains("magnetica") || nombre.Contains("panel")) { textoAmostrar = "[E] Usar Panel"; detectado = true; break; }
            }
        }

        
        if (detectado && textoAmostrar != "")
        {
            promptText.text = textoAmostrar;
            promptText.gameObject.SetActive(true);
            if (crosshair != null) crosshair.SetHighlight(true);
        }
        else
        {
            promptText.text = "";
            promptText.gameObject.SetActive(false);
            if (crosshair != null) crosshair.SetHighlight(false);
        }
    }

    private void InteractionTry()
    {
        if (cam == null) return;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null && InspectorManager.Instance != null)
            {
                InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                return;
            }

         
            ControladorPuerta puerta = hit.collider.GetComponentInParent<ControladorPuerta>();
            if (puerta != null)
            {
                puerta.IntentarAbrir();
                return;
            }

        
            CajaDeLuz cajaLuz = hit.collider.GetComponentInParent<CajaDeLuz>();
            if (cajaLuz != null)
            {
                if (TieneFusible && !cajaLuz.YaSeActivo)
                {
                    
                    cajaLuz.EncenderEnergia();
                }
                return;
            }

         
            Component[] componentes = hit.collider.GetComponentsInParent<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null || comp is Transform || comp is Collider || comp is MeshRenderer) continue;

                string nombre = comp.GetType().Name.ToLower();

               
                if (nombre.Contains("fusible"))
                {
                    TieneFusible = true;
                }

                comp.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Interaccion", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Recoger", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("AbrirCerradura", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}