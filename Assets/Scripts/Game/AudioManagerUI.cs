using UnityEngine;

public class AudioManagerUI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private AudioSource fuenteEfectos;

    [Header("Clips de Sonido")]
    [SerializeField] private AudioClip sonidoClick;
    [SerializeField] private AudioClip sonidoHover;

    [Header("Configuración")]
    [Range(0f, 1f)][SerializeField] private float volumenSfx = 0.8f;

    public void ReproducirClick()
    {
        if (sonidoClick != null && fuenteEfectos != null)
        {
            fuenteEfectos.PlayOneShot(sonidoClick, volumenSfx);
        }
        else
        {
            Debug.LogWarning("asignar clip audio");
        }
    }
    public void ReproducirHover()
    {
        if (sonidoHover != null && fuenteEfectos != null)
        {
            fuenteEfectos.PlayOneShot(sonidoHover, volumenSfx);
        }
    }

    public int ObtenerVolumenTruncado()
    {
        return (int)(volumenSfx * 100);
    }
}