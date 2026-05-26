using UnityEngine;

public class UVRevealable : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private Renderer revealRenderer;
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private float coneAngle = 30f;

    private void Start()
    {
        if (revealRenderer == null)
            revealRenderer = GetComponent<Renderer>();

        revealRenderer.enabled = false;
    }

    private void Update()
    {
        bool visible = IsUVHitting();
        if (revealRenderer.enabled != visible)
            revealRenderer.enabled = visible;
    }

    private bool IsUVHitting()
    {
        if (flashlight == null || !flashlight.IsUVActive) return false;

        Transform lightT = flashlight.LightTransform;
        if (lightT == null) return false;

        Vector3 toObject = transform.position - lightT.position;
        float distance = toObject.magnitude;
        if (distance > maxDistance) return false;

        float angle = Vector3.Angle(lightT.forward, toObject.normalized);
        return angle <= coneAngle;
    }
}
