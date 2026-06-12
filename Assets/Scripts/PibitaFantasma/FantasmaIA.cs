using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FantasmaIA : MonoBehaviour
{
    [Header("Componentes")]
    public Animator anim;
    public NavMeshAgent agente;
    public Transform jugador;

    [Header("Configuración")]
    public float tiempoDeVida = 10f;
    public float distanciaDeteccion = 10f;
    public float distanciaMuerte = 1.5f; 

    [Header("Audios")]
    public AudioSource audioSourceEfectos;
    public AudioSource audioSourceLoop;
    public AudioClip clipAparicion;
    public AudioClip clipRezar;
    public AudioClip clipCaminar;
    public AudioClip clipAtaque;
    public AudioClip clipDesvanecer;

    private bool activa = false;
    private bool atacando = false;

    public void Aparecer(Transform puntoSpawn)
    {
        transform.position = puntoSpawn.position;
        transform.rotation = puntoSpawn.rotation;
        gameObject.SetActive(true);

        activa = true;
        atacando = false;
        agente.isStopped = true;

        anim.Play("Zombie Stand Up", -1, 0f);

        if (audioSourceEfectos != null && clipAparicion != null)
        {
            audioSourceEfectos.PlayOneShot(clipAparicion);
        }

        StartCoroutine(CicloDeVida());
    }

    private IEnumerator CicloDeVida()
    {
        yield return new WaitForSeconds(1f);

        float tiempoPasado = 0f;
        float tiempoBuscando = tiempoDeVida - 1f; 

        
        while (tiempoPasado < tiempoBuscando && activa && !atacando)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);

            if (distancia <= distanciaMuerte)
            {
                Atacar();
                yield break; 
            }
            else if (distancia <= distanciaDeteccion)
            {
                agente.isStopped = false;
                agente.SetDestination(jugador.position);
                anim.SetBool("isWalking", true);
                ReproducirAudioLoop(clipCaminar);
            }
            else
            {
                agente.isStopped = true;
                anim.SetBool("isWalking", false);
                ReproducirAudioLoop(clipRezar);
            }

            tiempoPasado += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (activa && !atacando)
        {
            Desvanecer();
        }
    }

    private void ReproducirAudioLoop(AudioClip clip)
    {
        if (audioSourceLoop != null && audioSourceLoop.clip != clip)
        {
            audioSourceLoop.clip = clip;
            audioSourceLoop.loop = true;
            audioSourceLoop.Play();
        }
    }

    private void Atacar()
    {
        atacando = true;
        activa = false;
        agente.isStopped = true;

        if (audioSourceLoop != null) audioSourceLoop.Stop();
        if (audioSourceEfectos != null && clipAtaque != null) audioSourceEfectos.PlayOneShot(clipAtaque);

        anim.SetTrigger("Attack");
        StartCoroutine(GameOverSecuencia());
    }

    private void Desvanecer()
    {
        activa = false;
        agente.isStopped = true;

        if (audioSourceLoop != null) audioSourceLoop.Stop();
        if (audioSourceEfectos != null && clipDesvanecer != null) audioSourceEfectos.PlayOneShot(clipDesvanecer);

        anim.SetTrigger("Vanish");
        StartCoroutine(ApagarDespuesDeAnimacion());
    }

    private IEnumerator GameOverSecuencia()
    {
        yield return new WaitForSeconds(1.5f); 
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator ApagarDespuesDeAnimacion()
    {
        yield return new WaitForSeconds(2f); 
        gameObject.SetActive(false);
    }
}
