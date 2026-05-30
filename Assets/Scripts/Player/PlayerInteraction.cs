using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private bool cajaDescubierta = false;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    [Header("Crosshair")]
    [SerializeField] private CrosshairController crosshair;

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
        bool detectado = false;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            if (hit.collider.GetComponent<NotaInteractuable>())
            {
                promptText.text = "[E] Leer Nota";
                detectado = true;
            }
            else if (hit.collider.GetComponent<LlaveRecogible>())
            {
                promptText.text = "[E] Agarrar Llave";
                detectado = true;
            }
            else if (hit.collider.GetComponentInParent<ControladorPuerta>() != null)
            {
                promptText.text = "[E] Puerta";
                detectado = true;
            }
            else if (hit.collider.GetComponent<DemonDoll>())
            {
                promptText.text = "[E] Tocar Muñeca";
                detectado = true;
            }
            else if (hit.collider.GetComponent<ObjetoInspeccionable>())
            {
                promptText.text = "[E] Inspeccionar";
                detectado = true;
            }
            else if (hit.collider.GetComponent<Flashlight>())
            {
                promptText.text = "[E] Agarrar Linterna";
                detectado = true;
            }
            else if (hit.collider.GetComponent<FusibleRecogible>())
            {
                promptText.text = "[E] Agarrar Fusible";
                detectado = true;
            }
            else if (hit.collider.GetComponentInParent<KeypadFocus>() != null)
            {
                promptText.text = "[E] Usar Keypad";
                detectado = true;
            }
            else
            {
                CajaDeLuz caja = hit.collider.GetComponent<CajaDeLuz>();
                if (caja != null && !caja.YaSeActivo)
                {
                    promptText.text = TieneFusible ? "[E] Colocar Fusible" : "[E] Reactivar panel eléctrico";
                    detectado = true;
                }
            }
        }

        promptText.gameObject.SetActive(detectado);
        if (crosshair != null) crosshair.SetHighlight(detectado);
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
                fusible.Recoger();
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
                else
                {
                    if (!cajaDescubierta)
                    {
                        cajaDescubierta = true;
                        objectivePanel.Show("Encuentra el fusible en la cocina");
                    }
                }
                return;
            }

            KeypadFocus keypadFocus = hit.collider.GetComponent<KeypadFocus>();
            if (keypadFocus != null)
            {
                keypadFocus.Interactuar();
                return;
            }
        }
    }
}