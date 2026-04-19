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
            
            if (hit.collider.GetComponentInParent<GraspableObject>())
            {
                promptText.text = "Presiona [E] para agarrar";
                promptText.gameObject.SetActive(true);
                return;
            }

            // 2. Nota
            if (hit.collider.GetComponentInParent<NotaInteractuable>())
            {
                promptText.text = "Presiona [E] para leer nota";
                promptText.gameObject.SetActive(true);
                return;
            }

            // 3. Victoria
            if (hit.collider.GetComponentInParent<LlaveVictoria>())
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
        if (Physics.Raycast(cam.position, cam.forward, out hit, interactionDistance))
        {
          
            Debug.Log("Raycast impactó en: " + hit.collider.name);


            GraspableObject grasp = hit.collider.GetComponentInParent<GraspableObject>();
            if (grasp != null)
            {
                grasp.Take(cam, playerCollider);
                actualObjectInHand = grasp;
                return;
            }

            
            NotaInteractuable nota = hit.collider.GetComponentInParent<NotaInteractuable>();
            if (nota != null)
            {
                Debug.Log("Ejecutando Interactuar() en la nota...");
                nota.Interactuar();
                return;
            }

       
            LlaveVictoria victoria = hit.collider.GetComponentInParent<LlaveVictoria>();
            if (victoria != null)
            {
                victoria.Victoria();
                return;
            }
        }
    }
}