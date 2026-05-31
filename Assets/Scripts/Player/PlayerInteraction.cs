using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 

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

        if (Physics.Raycast(ray, out hit, distanciaInteraccion, capasInteractuables))
        {
            if (hit.collider.GetComponent<ObjetoInspeccionable>() != null)
            {
                if (textoPrompt != null) textoPrompt.text = "[E] Inspeccionar";
                return;
            }
        }

        if (textoPrompt != null) textoPrompt.text = "";
    }

    private void InteractionTry()
    {
        if (camaraJugador == null) return;

        Ray ray = new Ray(camaraJugador.transform.position, camaraJugador.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion, capasInteractuables))
        {
            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null)
            {
                if (InspectorManager.Instance != null)
                {
                    InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                }
                return;
            }

            FusibleRecogible fusible = hit.collider.GetComponent<FusibleRecogible>();
            if (fusible != null)
            {
                TieneFusible = true;
                fusible.Recoger();
                return;
            }

            CajaDeLuz caja = hit.collider.GetComponent<CajaDeLuz>();
            if (caja != null)
            {
                Debug.Log("Interactuando con caja de luz.");
                return;
            }
        }
    }
}