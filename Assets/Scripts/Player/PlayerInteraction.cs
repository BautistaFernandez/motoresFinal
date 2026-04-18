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
        UpdateUI();

        if (tryInteract)
        {
            InteractionTry();
            tryInteract = false;
        }
    }

    private void UpdateUI()
    {
        if (actualObjectInHand != null)
        {
            promptText.gameObject.SetActive(false);
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out GraspableObject foundObject))
            {
                promptText.text = "Press [E] to pick up";
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

        Debug.DrawRay(cam.position, cam.forward * interactionDistance, Color.red, 2f);

        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out GraspableObject foundObject))
            {
                foundObject.Take(cam, playerCollider);
                actualObjectInHand = foundObject;
            }
        }
    }
}
