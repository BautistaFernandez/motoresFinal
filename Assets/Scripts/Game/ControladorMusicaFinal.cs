using UnityEngine;

public class ControlSonidoFinal : MonoBehaviour
{
    public AudioSource audioSource; 
    public float volumenMaximo = 1f; 
    private float tiempoTotal = 600f;
    private float activacionSonido = 570f; 
    private float cronometro = 0f;
    private bool sonidoIniciado = false;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0.5f; 
        }
    }

    void Update()
    {
        cronometro += Time.deltaTime;
        if (cronometro >= activacionSonido && !sonidoIniciado)
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
            sonidoIniciado = true;
        }
        if (sonidoIniciado && cronometro <= tiempoTotal)
        {
            float tiempoEnAumento = cronometro - activacionSonido;
            float duracionAumento = tiempoTotal - activacionSonido; 

            audioSource.volume = Mathf.Lerp(0.5f, volumenMaximo, tiempoEnAumento / duracionAumento);
        }
        else if (cronometro > tiempoTotal)
        {
            audioSource.volume = volumenMaximo;
        }
    }
}