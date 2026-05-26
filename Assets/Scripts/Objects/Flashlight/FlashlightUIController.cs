using UnityEngine;

// ── PATRÓN OBSERVER ─────────────────────────────────────────────
public class FlashlightUIController : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private GameObject controlsPanel;

    //[Header("Pool de hints")]
    //[SerializeField] private HintPool hintPool;

    private void Start()
    {
        if (controlsPanel != null) controlsPanel.SetActive(false);

        if (flashlight != null)
            flashlight.OnPickedUp += HandleFlashlightPicked;
    }

    private void OnDestroy()
    {
        if (flashlight != null)
            flashlight.OnPickedUp -= HandleFlashlightPicked;
    }

    private void HandleFlashlightPicked()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
            //hintPool.ShowMessage("Parece que está linterna tiene un modo UV. Podré descubrir secretos en las paredes?", 7f);
        }
    }
}
