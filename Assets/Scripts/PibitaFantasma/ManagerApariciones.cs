using System.Collections;
using UnityEngine;

public class ManagerApariciones : MonoBehaviour
{
    [Header("Referencias")]
    public FantasmaIA scriptFantasma;
    public Transform[] puntosDeSpawn;

    [Header("Tiempos")]
    public float tiempoAparicion = 30f;

    private bool enCasaEvil = false;

    private void Start()
    {
        if (scriptFantasma != null)
        {
            scriptFantasma.gameObject.SetActive(false);
        }
    }

    public void EmpezarContadorEvil()
    {
        if (!enCasaEvil)
        {
            enCasaEvil = true;
            StartCoroutine(CicloDeSustos());
        }
    }

    public void FrenarContadorEvil()
    {
        enCasaEvil = false;
        StopAllCoroutines();

        if (scriptFantasma != null)
        {
            scriptFantasma.gameObject.SetActive(false);
        }
    }

    private IEnumerator CicloDeSustos()
    {
        while (enCasaEvil)
        {
            yield return new WaitForSeconds(tiempoAparicion);

            if (!scriptFantasma.gameObject.activeInHierarchy && puntosDeSpawn.Length > 0)
            {
                int indiceRandom = Random.Range(0, puntosDeSpawn.Length);

                if (scriptFantasma.agente != null) scriptFantasma.agente.enabled = false;

                scriptFantasma.Aparecer(puntosDeSpawn[indiceRandom]);
            }
        }
    }
}