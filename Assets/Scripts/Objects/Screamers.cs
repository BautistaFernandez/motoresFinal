using UnityEngine;

public class Screamers : MonoBehaviour
{

    [SerializeField] private AudioSource sounds;
    private bool wasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wasActivated)
        {
            ScreamerEvent();
        }
    }

    private void ScreamerEvent()
    {
        if (sounds != null) 
        {
            sounds.Play();
        }

        wasActivated = true;
        Destroy(gameObject, 2f);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
