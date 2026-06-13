using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private Transform playerCamera;

    [Header("Footsteps")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private float stepDistance = 2f;
    [SerializeField] private float minTimeBetweenSteps = 0.25f;
    [SerializeField] private float pitchVariation = 0.05f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Skin")]
    [SerializeField] private Transform playerSkin;
    [SerializeField] private float skinRotationSpeed = 10f;

    private Vector3 lastPosition;
    private float distanceTraveled;
    private float lastStepTime;
    private bool wasMoving;

    private Rigidbody rb;
    private Vector2 movementEntry;

    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        controls = new PlayerControls();

        controls.Player.Movement.performed += contexto => movementEntry = contexto.ReadValue<Vector2>();
        controls.Player.Movement.canceled += contexto => movementEntry = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lastPosition = transform.position;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void FixedUpdate()
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (right * movementEntry.x) + (forward * movementEntry.y);
        movement.Normalize();

        if (playerSkin != null)
        {
            Vector3 forwardCamara = playerCamera.forward;
            forwardCamara.y = 0;
            forwardCamara.Normalize();

            if (forwardCamara.sqrMagnitude > 0.01f)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(forwardCamara, Vector3.up);
                playerSkin.rotation = Quaternion.Slerp(playerSkin.rotation, rotacionDeseada, skinRotationSpeed * Time.fixedDeltaTime);
            }
        }

        Vector3 finalSpeed = movement * speed;
        finalSpeed.y = rb.linearVelocity.y;

        rb.linearVelocity = finalSpeed;

        Footsteps();

        if (animator != null)
        {
            bool isMoving = movementEntry.magnitude > 0.1f;
            animator.SetBool("IsWalking", isMoving);
        }
    }

    private void Footsteps()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = lastPosition.y;

        bool isMoving = movementEntry.magnitude > 0.1f;

        if (isMoving)
        {
            if (!wasMoving)
            {
                PlayStep();
                distanceTraveled = 0f;
            }
            else
            {
                distanceTraveled += Vector3.Distance(currentPosition, lastPosition);

                if (distanceTraveled >= stepDistance)
                {
                    PlayStep();
                    distanceTraveled = 0f;
                }
            }
        }
        else
        {
            distanceTraveled = 0f;
        }

        wasMoving = isMoving;
        lastPosition = transform.position;
    }

    private void PlayStep()
    {
        if (Time.time - lastStepTime < minTimeBetweenSteps) return;

        audioSource.pitch = 1f + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(footstep);
        lastStepTime = Time.time;
    }
}
