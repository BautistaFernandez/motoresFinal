using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SonidoTeleEstatica : MonoBehaviour
{
    private AudioSource audioSource;
    private float tiempoTranscurrido = 0f;

    [Range(0f, 1f)]
    [SerializeField] private float volumenMinimoInicial = 0.15f;

    [Range(0f, 1f)]
    [SerializeField] private float volumenMaximo = 0.8f;

    [SerializeField] private AudioClip pistaEstatica;

    private const float tiempoTotal = 600f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (pistaEstatica != null)
        {
            audioSource.clip = pistaEstatica;
        }

        audioSource.volume = volumenMinimoInicial;
    }

    private void Update()
    {
        if (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;

            float aumento = tiempoTranscurrido / tiempoTotal;
            aumento = Mathf.Clamp01(aumento);

            audioSource.volume = Mathf.Lerp(volumenMinimoInicial, volumenMaximo, aumento);
        }
    }
}