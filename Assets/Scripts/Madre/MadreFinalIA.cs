using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MadreFinalIA : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agente;
    public Animator anim;
    public Transform jugador;

    private bool puedePerseguir = false;

    void Start()
    {
        if (agente != null) agente.enabled = false;
    } 

    public void Despertar()
    {
        anim.SetTrigger("Scream");
        StartCoroutine(EsperarGritoYCorrer());
    }

    private IEnumerator EsperarGritoYCorrer()
    {
        yield return new WaitForSeconds(2.5f);

        if (agente != null)
        {
            agente.enabled = true;
            agente.isStopped = false;
        }
        puedePerseguir = true;
    }

    void Update()
    {
        if (puedePerseguir && jugador != null)
        {
            agente.SetDestination(jugador.position);

            if (Vector3.Distance(transform.position, jugador.position) <= 1.8f)
            {
                Matar();
            }
        }
    }

    void Matar()
    {
        puedePerseguir = false;
        agente.isStopped = true;
        agente.velocity = Vector3.zero;

        anim.SetTrigger("Attack");
        StartCoroutine(GameOverSecuencia());
    }

    private IEnumerator GameOverSecuencia()
    {
        yield return new WaitForSeconds(1.5f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOverScene");
    }
}