using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public Transform cam;
    public float interactionDistance = 8f;

    [Header("UI")]
    public TextMeshProUGUI promptText;

    private GraspableObject actualObjectInHand;
    private PlayerControls control;
    private bool tryInteract = false;
    private Collider playerCollider;

    private void Awake()
    {
        control = new PlayerControls();
        control.Player.Interaction.performed += context => tryInteract = true;
        playerCollider = GetComponent<Collider>();
    }

    private void OnEnable() => control.Enable();
    private void OnDisable() => control.Disable();

    private void Update()
    {
        if (promptText == null || cam == null) return;
        UpdateUI();
        if (tryInteract) { InteractionTry(); tryInteract = false; }
    }

    private void UpdateUI()
    {
        if (actualObjectInHand != null) { promptText.gameObject.SetActive(false); return; }

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            if (hit.collider.GetComponentInParent<DemonDoll>())
            {
                promptText.text = "Presiona [E] para activar muñeca";
                promptText.gameObject.SetActive(true);
                return;
            }
            if (hit.collider.GetComponentInParent<LlaveRecogible>())
            {
                promptText.text = "Presiona [E] para agarrar llave";
                promptText.gameObject.SetActive(true);
                return;
            }
            if (hit.collider.GetComponentInParent<NotaInteractuable>())
            {
                promptText.text = "Presiona [E] para leer nota";
                promptText.gameObject.SetActive(true);
                return;
            }
            if (hit.collider.GetComponentInParent<GraspableObject>())
            {
                promptText.text = "Presiona [E] para agarrar";
                promptText.gameObject.SetActive(true);
                return;
            }
        }
        promptText.gameObject.SetActive(false);
    }

    private void InteractionTry()
    {
        if (actualObjectInHand != null)
        {
            actualObjectInHand.Drop(playerCollider);
            actualObjectInHand = null;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            var doll = hit.collider.GetComponentInParent<DemonDoll>();
            if (doll != null)
            {
                var manager = Object.FindFirstObjectByType<DollEventManager>();
                if (manager != null) manager.IniciarContador();
                return;
            }

            var llave = hit.collider.GetComponentInParent<LlaveRecogible>();
            if (llave != null)
            {
                llave.Recoger();
                return;
            }

            var nota = hit.collider.GetComponentInParent<NotaInteractuable>();
            if (nota != null)
            {
                nota.Interactuar();
                return;
            }

            var grasp = hit.collider.GetComponentInParent<GraspableObject>();
            if (grasp != null)
            {
                grasp.Take(cam, playerCollider);
                actualObjectInHand = grasp;
            }
        }
    }
}