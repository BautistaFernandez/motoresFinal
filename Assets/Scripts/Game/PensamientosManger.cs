using UnityEngine;
using TMPro;
using System.Collections;

public class PensamientosManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoPensamiento;
    [SerializeField] private string[] listaPensamientos;
    [SerializeField] private float tiempoInicialPensamiento = 1f;
    [SerializeField] private float tiempoEntrePensamientos = 15f;
    [SerializeField] private float duracionEnPantalla = 4f;

    void Start()
    {
        if (textoPensamiento != null) textoPensamiento.gameObject.SetActive(false);
        StartCoroutine(RutinaPensamientos());
    }

    private IEnumerator RutinaPensamientos()
    {
        int indice = 0;
        while (true)
        {
            yield return new WaitForSeconds(tiempoInicialPensamiento);

            if (listaPensamientos.Length > 0 && textoPensamiento != null)
            {
                textoPensamiento.text = listaPensamientos[indice];
                textoPensamiento.gameObject.SetActive(true);
                yield return new WaitForSeconds(duracionEnPantalla);
                textoPensamiento.gameObject.SetActive(false);
                indice = (indice + 1) % listaPensamientos.Length;
            }
            yield return new WaitForSeconds(tiempoEntrePensamientos);
        }
    }
}