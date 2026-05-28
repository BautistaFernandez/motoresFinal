using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private TextMeshProUGUI promptText;

    public bool TieneFusible { get; set; } = false;

    void Update()
    {
        UpdateUI();
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractionTry();
        }
    }

    private void UpdateUI()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
           
            if (hit.collider.GetComponent<NotaInteractuable>()) { promptText.text = "[E] Leer Nota"; promptText.gameObject.SetActive(true); return; }
            if (hit.collider.GetComponent<LlaveRecogible>()) { promptText.text = "[E] Agarrar Llave"; promptText.gameObject.SetActive(true); return; }

            ControladorPuerta puerta = hit.collider.GetComponentInParent<ControladorPuerta>();
            if (puerta != null) { promptText.text = "[E] Puerta"; promptText.gameObject.SetActive(true); return; }

            if (hit.collider.GetComponent<DemonDoll>()) { promptText.text = "[E] Tocar Muñeca"; promptText.gameObject.SetActive(true); return; }

            if (hit.collider.GetComponent<ObjetoInspeccionable>()) { promptText.text = "[E] Inspeccionar"; promptText.gameObject.SetActive(true); return; }

            if (hit.collider.GetComponent<Flashlight>()) { promptText.text = "[E] Agarrar Linterna"; promptText.gameObject.SetActive(true); return; }

            
            if (hit.collider.GetComponent<FusibleRecogible>())
            {
                promptText.text = "[E] Agarrar Fusible";
                promptText.gameObject.SetActive(true);
                return;
            }

            CajaDeLuz caja = hit.collider.GetComponent<CajaDeLuz>();
            if (caja != null)
            {
               
                if (caja.YaSeActivo)
                {
                    promptText.gameObject.SetActive(false);
                    return;
                }

               
                if (TieneFusible) promptText.text = "[E] Colocar Fusible y Subir Perilla";
                else promptText.text = "Falta el Fusible en la caja...";

                promptText.gameObject.SetActive(true);
                return;
            }

            promptText.gameObject.SetActive(false);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void InteractionTry()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
           
            LlaveRecogible llave = hit.collider.GetComponent<LlaveRecogible>();
            if (llave != null)
            {
                llave.Recoger();
                return;
            }

            ControladorPuerta puerta = hit.collider.GetComponentInParent<ControladorPuerta>();
            if (puerta != null)
            {
                puerta.IntentarAbrir();
                return;
            }

            if (hit.collider.GetComponent<DemonDoll>())
            {
                var manager = Object.FindFirstObjectByType<DollEventManager>();
                if (manager != null) manager.IniciarContador();
                return;
            }

            Flashlight linterna = hit.collider.GetComponent<Flashlight>();
            if (linterna != null)
            {
                linterna.Recoger();
                return;
            }

            NotaInteractuable nota = hit.collider.GetComponent<NotaInteractuable>();
            if (nota != null)
            {
                nota.Interactuar();
                return;
            }

            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null)
            {
                inspeccionable.Inspeccionar();
                return;
            }

           
            FusibleRecogible fusible = hit.collider.GetComponent<FusibleRecogible>();
            if (fusible != null)
            {
                TieneFusible = true;
                Destroy(fusible.gameObject);
                return;
            }

            CajaDeLuz caja = hit.collider.GetComponent<CajaDeLuz>();
            if (caja != null)
            {
                
                if (caja.YaSeActivo) return;

                if (TieneFusible)
                {
                    TieneFusible = false;
                    caja.EncenderEnergia();
                }
                return;
            }
        }
    }
}