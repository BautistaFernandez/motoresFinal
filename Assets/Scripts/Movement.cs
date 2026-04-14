using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private float speed = 3f;

    private Rigidbody rb;
    private Vector2 movementEntry;

    private PlayerControls controls;
    //public Animator animator;

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
        Vector3 movement = (transform.right * movementEntry.x) + (transform.forward * movementEntry.y);

        //animator.SetFloat("Speed", movement.magnitude);

        movement.Normalize();

        Vector3 finalSpeed = movement * speed;

        finalSpeed.y = rb.linearVelocity.y;

        rb.linearVelocity = finalSpeed;
    }
}
