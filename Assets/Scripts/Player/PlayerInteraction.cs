using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public Transform cam;
    public float interactionDistance = 4f; 

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
            
            if (hit.collider.TryGetComponent(out GraspableObject foundGraspable))
            {
                promptText.text = "Presiona [E] para agarrar";
                promptText.gameObject.SetActive(true);
                return;
            }

           
            if (hit.collider.TryGetComponent(out NotaInteractuable foundNota))
            {
                promptText.text = "Presiona [E] para leer nota";
                promptText.gameObject.SetActive(true);
                return;
            }

         
            if (hit.collider.TryGetComponent(out LlaveVictoria foundVictoria))
            {
                promptText.text = "Presiona [E] para escapar";
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
           
            if (hit.collider.TryGetComponent(out GraspableObject foundGraspable))
            {
                foundGraspable.Take(cam, playerCollider);
                actualObjectInHand = foundGraspable;
            }
           
            else if (hit.collider.TryGetComponent(out NotaInteractuable foundNota))
            {
                foundNota.Interactuar();
            }
            
            else if (hit.collider.TryGetComponent(out LlaveVictoria foundVictoria))
            {
                foundVictoria.Victoria();
            }
        }
    }
}