using UnityEngine;
using UnityEngine.InputSystem;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        private Camera cam;

        private void Awake() => cam = Camera.main;

        private void Update()
        {
            if (Mouse.current == null) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Ray ray = cam.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        keypadButton.PressButton();
                    }
                }
            }
        }
    }
}