using UnityEngine;

public class DemonDoll : MonoBehaviour
{
    /*private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }*/

    public void Aparision()
    {
        gameObject.SetActive(true);

        /*if (rb != null )
        {
            rb.isKinematic = false;
        }*/
        // GetComponent<AudioSource>().Play();
        // GetComponent<Animator>().SetTrigger("Susto");
        Debug.Log("Evento: Aparece la doll");
    }

    public void Invisible()
    {
        gameObject.SetActive(false);
    }

    /*public void GravityActive()
    {
        if (rb != null)
        {
            rb.isKinematic = ;
        }
    }*/
}

