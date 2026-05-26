using System.Collections;
using UnityEngine;

// ── HERENCIA: InteractableObject ─────────────────────────────────────────────
// Al presionar E cerca del keypad, la cámara hace un tween a focusPoint.
// El cursor se libera para poder clickear los botones del keypad.
// Con ESC la cámara vuelve a su posición original y el cursor se lockea.
// Si el código se ingresa correctamente, Door.UnlockByCode() termina el loop,
// y LevelManager teletransporta al player a playerOutside antes del reset.
// ─────────────────────────────────────────────────────────────────────────────
public class KeypadFocus : InteractableObject
{
    [Header("Cámara")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform focusPoint;   // vacío posicionado frente al keypad (mirándolo)
    [SerializeField] private float transitionSpeed = 6f;

    [Header("Scripts a pausar mientras se usa el keypad")]
    [SerializeField] private MonoBehaviour cameraLook;
    [SerializeField] private MonoBehaviour playerMovement;

    [Header("Script de click del keypad a activar")]
    [SerializeField] private MonoBehaviour keypadInteraction;

    [Header("Pool de hints")]
    [SerializeField] private HintPool hintPool;

    private Vector3 originalCamLocalPos;
    private Quaternion originalCamLocalRot;
    private Transform originalCamParent;
    private bool focused = false;
    private bool transitioning = false;
    private bool firstLook = false;

    private void Start()
    {
        // El KeypadInteractionFPV debe arrancar apagado para que no clickee fuera del keypad.
        if (keypadInteraction != null) keypadInteraction.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (focused && Input.GetKeyDown(KeyCode.Escape) && !transitioning)
            StartCoroutine(ExitFocus());
    }

    protected override void OnInteract()
    {
        if (focused || transitioning) return;
        StartCoroutine(EnterFocus());
    }

    private IEnumerator EnterFocus()
    {
        transitioning = true;
        HideUI();


        if (!firstLook) hintPool.ShowMessage("Cuando puse sistema de seguridad? Necesito un código de 3 dígitos para poder irme", 6f);
        firstLook = true;

        originalCamParent = cam.parent;
        originalCamLocalPos = cam.localPosition;
        originalCamLocalRot = cam.localRotation;

        if (cameraLook != null) cameraLook.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;

        cam.SetParent(null);

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

        // Liberar cursor y activar click en botones
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (cameraLook != null) cameraLook.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;

        focused = false;
        transitioning = false;
    }

    public void ForceExitImmediate()
    {
        if (!focused && !transitioning) return;

        StopAllCoroutines();

        if (keypadInteraction != null) keypadInteraction.enabled = false;

        cam.SetParent(originalCamParent);
        cam.localPosition = originalCamLocalPos;
        cam.localRotation = originalCamLocalRot;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (cameraLook != null) cameraLook.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;

        focused = false;
        transitioning = false;
    }
}
