using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Luz de la linterna")]
    [SerializeField] private Light flashlightLight;

    [Header("Posición al agarrarla")]
    [SerializeField] private Transform holderOnPickup;

    [Header("Audio")]
    [SerializeField] private AudioSource toggleSound;

    [Header("Rotación al recoger")]
    [SerializeField] private Vector3 pickupRotation = Vector3.zero;

    private ILightMode currentMode;
    private ILightMode normalMode;
    private ILightMode uvMode;

    private bool picked = false;
    private bool turnedOn = false;

    public bool IsPicked => picked;
    public bool IsUVActive => picked && turnedOn && currentMode.IsUVMode;
    public Transform LightTransform => flashlightLight != null ? flashlightLight.transform : null;

    // Evento que disparan los suscriptores al recoger la linterna.
    public event Action OnPickedUp;

    private void Awake()
    {
        normalMode = new NormalLightMode();
        uvMode = new UVLightMode();
        currentMode = normalMode;

        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    private void Update()
    {
        if (!picked || Keyboard.current == null) return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleLight();
        }

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            SwitchMode();
        }
    }

    public void Recoger()
    {
        if (picked) return;

        picked = true;

        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
        if (TryGetComponent(out Collider col)) col.enabled = false;

        if (holderOnPickup != null)
        {
            transform.SetParent(holderOnPickup);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(pickupRotation);
        }

        OnPickedUp?.Invoke();
    }

    private void ToggleLight()
    {
        turnedOn = !turnedOn;
        if (flashlightLight != null) flashlightLight.enabled = turnedOn;

        if (turnedOn) currentMode.Apply(flashlightLight);

        if (toggleSound != null) toggleSound.Play();
    }

    private void SwitchMode()
    {
        if (!turnedOn) return;

        currentMode = currentMode.IsUVMode ? normalMode : uvMode;
        currentMode.Apply(flashlightLight);

        if (toggleSound != null) toggleSound.Play();
    }
}
