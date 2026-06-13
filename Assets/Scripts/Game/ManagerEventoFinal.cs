using UnityEngine;

public class ManagerEventoFinal : MonoBehaviour
{
    [Header("Las 4 Madres")]
    public MadreFinalIA[] madres;

    [Header("Audios")]
    public AudioSource sourceMundo;
    public AudioSource sourcePlayer; 

    public AudioClip musicaTension;
    public AudioClip gritoFuerte;
    public AudioClip latidosCorazon;
    public AudioClip respiracionAgitada;

 
    public void DesatarElCaos()
    {
      
        if (sourceMundo != null)
        {
            sourceMundo.PlayOneShot(gritoFuerte);
            sourceMundo.clip = musicaTension;
            sourceMundo.loop = true;
            sourceMundo.PlayDelayed(1.5f);
        }

       
        if (sourcePlayer != null)
        {
            sourcePlayer.PlayOneShot(latidosCorazon);
            sourcePlayer.clip = respiracionAgitada;
            sourcePlayer.loop = true;
            sourcePlayer.Play();
        }

        foreach (MadreFinalIA madre in madres)
        {
            if (madre != null) madre.Despertar();
        }
    }
}