using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    [Header("Fusible")]
    [SerializeField] private GameObject fusibleEnEscena;
    public bool TieneFusible { get; set; } = false;

    [Header("Keypad")]
    [SerializeField] private NavKeypad.Keypad keypad;

    private bool keypadResuelto = false;

    private void Start()
    {
        if (keypad != null) keypad.OnCodigoCorrecto += HabilitarCajaDeLuz;
        if (fusibleEnEscena != null) fusibleEnEscena.SetActive(false);
    }

    private void OnDestroy()
    {
        if (keypad != null) keypad.OnCodigoCorrecto -= HabilitarCajaDeLuz;
    }

    private void HabilitarCajaDeLuz()
    {
        keypadResuelto = true;
    }

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
                    detectado = true;
                    if (keypadResuelto)
                    {
                        promptText.text = TieneFusible ? "[E] Colocar Fusible" : "[E] Reactivar panel eléctrico";
                    }
                    else
                    {
                        promptText.text = "";
                    }
                }
            }
        }

        promptText.gameObject.SetActive(detectado);
        if (crosshair != null) crosshair.SetHighlight(detectado);
    }

    private void InteractionTry()
    {
        if (cam == null) return;

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

            ObjetoInspeccionable inspeccionable = hit.collider.GetComponent<ObjetoInspeccionable>();
            if (inspeccionable != null && InspectorManager.Instance != null)
            {
                InspectorManager.Instance.IniciarInspeccion(inspeccionable);
                return;
            }

            Flashlight linterna = hit.collider.GetComponent<Flashlight>();
            if (linterna != null)
            {
                linterna.Recoger();
                return;
            }

            FusibleRecogible fusible = hit.collider.GetComponent<FusibleRecogible>();
            if (fusible != null)
            {
                fusible.Recoger();
                TieneFusible = true;
                return;
            }

            KeypadFocus keypad = hit.collider.GetComponentInParent<KeypadFocus>();
            if (keypad != null)
            {
                keypad.Interactuar();
                return;
            }

            CajaDeLuz cajaInteract = hit.collider.GetComponent<CajaDeLuz>();
            if (cajaInteract != null && !cajaInteract.YaSeActivo && keypadResuelto)
            {
                if (TieneFusible)
                {
                    cajaInteract.EncenderEnergia();
                }
                else
                {
                    if (!cajaDescubierta)
                    {
                        cajaDescubierta = true;
                        objectivePanel.Show("Encuentra el fusible en la cocina");
                        fusibleEnEscena.gameObject.SetActive(true);
                    }
                }
                return;
            }

            NotaInteractuable nota = hit.collider.GetComponent<NotaInteractuable>();
            if (nota != null) nota.Interactuar();
        }
    }
}