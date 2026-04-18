using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GraspableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalScale = transform.localScale;
    }

    public void Take(Transform cam, Collider playerCollider)
    {
        // Desactivar colisión con el personaje
        Physics.IgnoreCollision(col, playerCollider, true);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Posicionamos en world space antes de parentar para evitar distorsión de escala
        transform.position = cam.position + cam.forward * 1.5f + cam.up * -0.3f;
        transform.rotation = cam.rotation;

        transform.SetParent(cam);

        // Restauramos la escala original por si se distorsionó
        transform.localScale = originalScale;
    }

    public void Drop(Collider playerCollider)
    {
        transform.SetParent(null);

        // Restaurar escala por si acaso
        transform.localScale = originalScale;

        rb.isKinematic = false;

        // Reactivar colisión con el personaje
        Physics.IgnoreCollision(col, playerCollider, false);
    }
}
