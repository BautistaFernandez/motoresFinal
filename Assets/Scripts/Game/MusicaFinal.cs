using UnityEngine;

public class MusicaFinal : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volumen = 0.6f;

    private bool sonidoIniciado = false;

    private void OnEnable()
    {
        CajaDeLuz.OnEnergiaRestaurada += ActivarSonidoFinal;
    }

    private void OnDisable()
    {
        CajaDeLuz.OnEnergiaRestaurada -= ActivarSonidoFinal;
    }

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.loop = true;
        }
    }

    void Update()
    {
        if (sonidoIniciado && audioSource != null)
        {
            audioSource.volume = volumen;
            audioSource.pitch = 1f + Mathf.Sin(Time.time * 4f) * 0.12f;
        }
    }

    public void ActivarSonidoFinal()
    {
        if (!sonidoIniciado && audioSource != null)
        {
            audioSource.Play();
            sonidoIniciado = true;
        }
    }
}