using System.Collections;
using UnityEngine;

public class ManagerApariciones : MonoBehaviour
{
    [Header("Referencias")]
    public FantasmaIA scriptFantasma;
    public Transform[] puntosDeSpawn;

    [Header("Tiempos")]
    public float tiempoAparicion = 30f; 

    private void Start()
    {
       
        if (scriptFantasma != null)
        {
            scriptFantasma.gameObject.SetActive(false);
            StartCoroutine(CicloDeSustos());
        }
    }

    private IEnumerator CicloDeSustos()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(tiempoAparicion);

            
            if (!scriptFantasma.gameObject.activeInHierarchy && puntosDeSpawn.Length > 0)
            {
               
                int indiceRandom = Random.Range(0, puntosDeSpawn.Length);
                scriptFantasma.Aparecer(puntosDeSpawn[indiceRandom]);
            }
        }
    }
}