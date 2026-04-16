using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // ¡No olvides este namespace!

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public Transform cam;
    public float interactionDistance = 3f;
    public float raycastHeightOffset = 1.5f; // Altura desde donde sale el rayo (ojos/pecho)

    [Header("UI")]
    public TextMeshProUGUI promptText; // Arrastra aquí tu texto de la interfaz

    private GraspableObject actualObjectInHand;
    private PlayerControls control;

    private void Awake()
    {
        control = new PlayerControls();

        control.Player.Interaction.performed += context => InteractionTry();
    }

    private void OnEnable() => control.Enable();
    private void OnDisable() => control.Disable();

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Si ya tenemos algo en la mano, no mostramos el aviso de "Presionar E"
        if (actualObjectInHand != null)
        {
            promptText.gameObject.SetActive(false);
            return;
        }

        // Lanzamos el rayo para ver si estamos mirando un objeto agarrable
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out GraspableObject foundObject))
            {
                promptText.text = "Press [E] to pick up";
                promptText.gameObject.SetActive(true);
                return;
            }
        }

        // Si el rayo no toca nada o no es agarrable, ocultamos el texto
        promptText.gameObject.SetActive(false);
    }

    private void InteractionTry()
    {
        if (actualObjectInHand != null)
        {
            actualObjectInHand.Drop();
            actualObjectInHand = null;
            return;
        }

        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        RaycastHit hit;

        // Dibujamos el rayo en la consola de escena para debuguear
        Debug.DrawRay(rayOrigin, transform.forward * interactionDistance, Color.red, 2f);

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent(out GraspableObject foundObject))
            {
                foundObject.Take(cam);
                actualObjectInHand = foundObject;
            }
        }
    }
}
