using UnityEngine;

public class DemonDoll : MonoBehaviour
{ 
    public void Aparision()
    {
        gameObject.SetActive(true);
        // GetComponent<AudioSource>().Play();
        // GetComponent<Animator>().SetTrigger("Susto");
        Debug.Log("La muñeca dice: ¡Aquí estoy!");
    }

    public void Invisible()
    {
        gameObject.SetActive(false);
    }
}
