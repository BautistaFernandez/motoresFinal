using System;
using UnityEngine;

public class Flashlight : Pickable
{
    [Header("Luz de la linterna")]
    [SerializeField] private Light flashlightLight;

    [Header("Posición al agarrarla (hijo de la cámara)")]
    [SerializeField] private Transform holderOnPickup;

    [Header("Audio")]
    [SerializeField] private AudioSource toggleSound;

    private ILightMode currentMode;
    private ILightMode normalMode;
    private ILightMode uvMode;

    private bool picked = false;
    private bool turnedOn = false;

    public bool IsPicked => picked;
    public bool IsUVActive => picked && turnedOn && currentMode.IsUVMode;
    public Transform LightTransform => flashlightLight != null ? flashlightLight.transform : null;

    // Evento que disparan los suscriptores (UI, tutoriales, etc.) al recoger la linterna.
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
        if (!picked) return;

        if (Input.GetKeyDown(KeyCode.F))
            ToggleLight();

        if (Input.GetMouseButtonDown(1))
            SwitchMode();
    }

    public override string GetPrompt() => "Press [E] to take flashlight";

    public override void OnPickup()
    {
        if (picked) return;

        picked = true;

        if (TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;
        if (TryGetComponent(out Collider col))
            col.enabled = false;

        // Anclar a la cámara.
        if (holderOnPickup != null)
        {
            transform.SetParent(holderOnPickup);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        OnPickedUp?.Invoke();
    }

    private void ToggleLight()
    {
        turnedOn = !turnedOn;
        if (flashlightLight != null)
            flashlightLight.enabled = turnedOn;

        if (turnedOn)
            currentMode.Apply(flashlightLight);

        toggleSound?.Play();
    }

    private void SwitchMode()
    {
        if (!turnedOn) return;

        currentMode = currentMode.IsUVMode ? normalMode : uvMode;
        currentMode.Apply(flashlightLight);

        toggleSound?.Play();
    }
}
