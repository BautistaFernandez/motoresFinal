using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

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
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(movementEntry.x, 0f, movementEntry.y);

        Vector3 finalSpeed = direction * speed;

        finalSpeed.y = rb.linearVelocity.y;

        rb.linearVelocity = finalSpeed;
    }
}
