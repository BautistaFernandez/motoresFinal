using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GraspableObject : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Agarrar objecto
    public void Take(Transform cam)
    {
        rb.isKinematic = true;

        transform.SetParent(cam);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Soltar objecto
    public void Drop()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
    }
}
