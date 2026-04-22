using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private Transform playerCamera;

    [Header("Footsteps")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private float stepDistance = 2f;
    [SerializeField] private float minTimeBetweenSteps = 0.25f;
    [SerializeField] private float pitchVariation = 0.05f;

    private Vector3 lastPosition;
    private float distanceTraveled;
    private float lastStepTime;
    private bool wasMoving;

    private Rigidbody rb;
    private Vector2 movementEntry;
    private Vector2 lookEntry;
    private float xRotation = 0f;

    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        controls = new PlayerControls();

        controls.Player.Movement.performed += contexto => movementEntry = contexto.ReadValue<Vector2>();
        controls.Player.Movement.canceled += contexto => movementEntry = Vector2.zero;

        controls.Player.Look.performed += contexto => lookEntry = contexto.ReadValue<Vector2>();
        controls.Player.Look.canceled += contexto => lookEntry = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lastPosition = transform.position;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        RotarCamara();
    }

    private void FixedUpdate()
    {
        Vector3 movement = (transform.right * movementEntry.x) + (transform.forward * movementEntry.y);
        movement.Normalize();

        Vector3 finalSpeed = movement * speed;
        finalSpeed.y = rb.linearVelocity.y;

        rb.linearVelocity = finalSpeed;

        Footsteps();
    }

    private void RotarCamara()
    {
        float mouseX = lookEntry.x * sensitivity;
        float mouseY = lookEntry.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void Footsteps()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = lastPosition.y;

        bool isMoving = movementEntry.magnitude > 0.1f;

        if (isMoving)
        {
            // Primer paso al arrancar desde quieto
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
        // Debounce: evita que dos pasos suenen demasiado juntos
        if (Time.time - lastStepTime < minTimeBetweenSteps) return;

        audioSource.pitch = 1f + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(footstep);
        lastStepTime = Time.time;
    }
}
