using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Sistema moderno obligatorio para Unity 6

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capasInteractuables;
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private TextMeshProUGUI textoPrompt;

    private bool TieneFusible = false;

    private void Update()
    {
        ActualizarRaycastYPrompt();

        // Control unificado del Input System Moderno para la tecla E
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractionTry();
        }
    }

    private void ActualizarRaycastYPrompt()
    {
        if (camaraJugador == null) return;

        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;

        // Lanzamos el raycast libre de filtros para que no te bloquee ninguna puerta ni fusible
        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            // 1. Si es un objeto de inspección (Cartas, Fotos, Dibujo de la cocina)
            if (hit.collider.GetComponent<ObjetoInspeccionable>() != null)
            {
                if (textoPrompt != null) textoPrompt.text = "[E] Inspeccionar";
                return;
            }

            // 2. ESCÁNER UNIVERSAL DINÁMICO:
            // Buscamos de forma segura si el objeto tiene cualquier script relacionado a mecánicas
            // sin nombrar la clase directamente, evitando errores de compilación CS0246.
            Component[] componentes = hit.collider.GetComponents<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null) continue;
                string nombreScript = comp.GetType().Name.ToLower();

                if (nombreScript.Contains("puerta"))
                {
                    if (textoPrompt != null) textoPrompt.text = "[E] Abrir Puerta";
                    return;
                }
                if (nombreScript.Contains("fusible") && nombreScript.Contains("recog"))
                {
                    if (textoPrompt != null) textoPrompt.text = "[E] Recoger Fusible";
                    return;
                }
                if (nombreScript.Contains("luz") || nombreScript.Contains("caja"))
                {
                    if (textoPrompt != null) textoPrompt.text = "[E] Colocar Fusible";
                    return;
                }
                if (nombreScript.Contains("codigo") || nombreScript.Contains("cerradura") || nombreScript.Contains("panel"))
                {
                    if (textoPrompt != null) textoPrompt.text = "[E] Introducir Código";
                    return;
                }
            }
        }

        // Si no estás apuntando a nada interactuable, limpiamos el prompt de la pantalla
        if (textoPrompt != null) textoPrompt.text = "";
    }

    private void InteractionTry()
    {
        if (camaraJugador == null) return;

        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            // A. Sistema de Inspección Base (Se mantiene intacto el cambio de casa aquí)
            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null)
            {
                if (InspectorManager.Instance != null)
                {
                    InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                }
                return;
            }

            // B. DISPARADOR DE SEÑALES UNIVERSAL:
            // Le manda la orden de ejecutarse a cualquier función de interacción que tengan los scripts 
            // de las puertas, fusibles, cajas de luz o paneles de tu compañero de forma segura.
            Component[] componentes = hit.collider.GetComponents<Component>();
            foreach (var comp in componentes)
            {
                if (comp == null || comp is Transform) continue;

                // Si es un fusible nativo tuyo, cambiamos el estado interno
                if (comp.GetType().Name.ToLower().Contains("fusible") && comp.GetType().Name.ToLower().Contains("recog"))
                {
                    TieneFusible = true;
                }

                // Ejecuta de forma segura cualquiera de los métodos comunes de interacciones de tu grupo
                comp.SendMessage("Interactuar", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Interaccion", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("AbrirPuerta", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("AbrirCerradura", SendMessageOptions.DontRequireReceiver);
                comp.SendMessage("Recoger", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}