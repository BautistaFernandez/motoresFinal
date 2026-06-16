using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadFocus : MonoBehaviour
{
    [Header("Cámara")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float transitionSpeed = 6f;
    [SerializeField] private Unity.Cinemachine.CinemachineInputAxisController cinemachineInput;
    [SerializeField] private Unity.Cinemachine.CinemachineCamera cinemachineFPSCam;

    [Header("Scripts a pausar")]
    [SerializeField] private Movement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;

    [Header("Script de click del keypad a activar")]
    [SerializeField] private NavKeypad.KeypadInteractionFPV keypadInteraction;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [TextArea]
    [SerializeField] private string mensajePrimerUso = "Encuentra el código para abrir y buscar la llave";

    private Vector3 originalCamLocalPos;
    private Quaternion originalCamLocalRot;
    private Transform originalCamParent;
    private bool focused = false;
    private bool transitioning = false;
    private bool firstLook = false;

    private void Start()
    {
        if (keypadInteraction != null) keypadInteraction.enabled = false;
    }

    private void Update()
    {
        if (focused && !transitioning && Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartCoroutine(ExitFocus());
        }
    }

    public void Interactuar()
    {
        if (focused || transitioning) return;
        StartCoroutine(EnterFocus());
    }

    private IEnumerator EnterFocus()
    {
        transitioning = true;

        if (!firstLook && objectivePanel != null && !string.IsNullOrEmpty(mensajePrimerUso))
        {
            objectivePanel.Show(mensajePrimerUso);
        }
        firstLook = true;

        originalCamParent = cam.parent;
        originalCamLocalPos = cam.localPosition;
        originalCamLocalRot = cam.localRotation;

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerInteraction != null) playerInteraction.enabled = false;
        if (cinemachineInput != null) cinemachineInput.enabled = false;

        cam.SetParent(null);

        if (cinemachineFPSCam != null) cinemachineFPSCam.gameObject.SetActive(false);

        float t = 0f;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            cam.position = Vector3.Lerp(startPos, focusPoint.position, t);
            cam.rotation = Quaternion.Slerp(startRot, focusPoint.rotation, t);
            yield return null;
        }
        cam.position = focusPoint.position;
        cam.rotation = focusPoint.rotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (keypadInteraction != null) keypadInteraction.enabled = true;

        focused = true;
        transitioning = false;
    }

    private IEnumerator ExitFocus()
    {
        transitioning = true;

        if (keypadInteraction != null) keypadInteraction.enabled = false;

        Vector3 targetWorldPos = originalCamParent != null
            ? originalCamParent.TransformPoint(originalCamLocalPos)
            : originalCamLocalPos;
        Quaternion targetWorldRot = originalCamParent != null
            ? originalCamParent.rotation * originalCamLocalRot
            : originalCamLocalRot;

        float t = 0f;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            cam.position = Vector3.Lerp(startPos, targetWorldPos, t);
            cam.rotation = Quaternion.Slerp(startRot, targetWorldRot, t);
            yield return null;
        }

        cam.SetParent(originalCamParent);
        cam.localPosition = originalCamLocalPos;
        cam.localRotation = originalCamLocalRot;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (cinemachineFPSCam != null) cinemachineFPSCam.gameObject.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerInteraction != null) playerInteraction.enabled = true;
        if (cinemachineInput != null) cinemachineInput.enabled = true;

        focused = false;
        transitioning = false;
    }
}
