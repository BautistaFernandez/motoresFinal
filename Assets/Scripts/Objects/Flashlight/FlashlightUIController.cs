using UnityEngine;

public class FlashlightUIController : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private GameObject hintEncender;
    [SerializeField] private GameObject hintMode;

    //[Header("Pool de hints")]
    //[SerializeField] private HintPool hintPool;

    private void Start()
    {
        if (hintEncender != null) hintEncender.SetActive(false);

        if (hintMode != null) hintMode.SetActive(false);

        if (flashlight != null)
            flashlight.OnPickedUp += HandleFlashlightPicked;
    }

    private void OnDestroy()
    {
        if (flashlight != null) flashlight.OnPickedUp -= HandleFlashlightPicked;
    }

    private void HandleFlashlightPicked()
    {
        if (hintEncender != null) hintEncender.SetActive(true);
        if (hintMode != null) hintMode.SetActive(true);
    }
}
