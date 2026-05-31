using System.Collections;
using TMPro;
using UnityEngine;

public class ObjetoInspeccionable : MonoBehaviour
{
    [Header("Configuración Base de Inspección")]
    [SerializeField] private Vector3 rotacionInicial;
    [SerializeField] private float distanciaCamara = 2f;

    [Header("UI de Subtítulos y Prompts")]
    [SerializeField] protected TextMeshProUGUI subtituloText;
    [Tooltip("Opcional: Arrastrá acá el cartel UI que dice [ESC] para salir")]
    [SerializeField] protected GameObject promptDejarText;
    [TextArea]
    [SerializeField] private string[] lineasSubtitulos;
    [SerializeField] private float velocidadTexto = 0.05f;

    private Coroutine corrutinaSubtitulos;

    protected virtual void AlInspeccionar() { }
    protected virtual void AlTerminarInspeccion() { }

    private bool DeberiaAutoCerrarse()
    {
        return GetComponent<DibujoDisparadorIntro>() == null;
    }

    public void IniciarInspeccion()
    {
        if (promptDejarText != null) promptDejarText.SetActive(true);

        AlInspeccionar();

        if (lineasSubtitulos != null && lineasSubtitulos.Length > 0 && subtituloText != null)
        {
            subtituloText.gameObject.SetActive(true);
            corrutinaSubtitulos = StartCoroutine(MostrarSubtitulos());
        }
    }

    private IEnumerator MouseSubtitulos()
    {
        foreach (string linea in lineasSubtitulos)
        {
            subtituloText.text = "";
            foreach (char letra in linea.ToCharArray())
            {
                subtituloText.text += letra;
                yield return new WaitForSeconds(velocidadTexto);
            }
            yield return new WaitForSeconds(2f);
        }

        // Si es una foto de pista normal, se cierra sola. Si es el dibujo, espera al ESC
        if (DeberiaAutoCerrarse())
        {
            subtituloText.text = "";
            subtituloText.gameObject.SetActive(false);
            if (promptDejarText != null) promptDejarText.SetActive(false);
            InspectorManager.Instance.TerminarInspeccionExterior();
        }
    }

    private IEnumerator MostrarSubtitulos()
    {
        return MouseSubtitulos();
    }

    public void TerminarInspeccion()
    {
        if (corrutinaSubtitulos != null) StopCoroutine(corrutinaSubtitulos);

        if (subtituloText != null)
        {
            subtituloText.text = "";
            subtituloText.gameObject.SetActive(false);
        }

        if (promptDejarText != null) promptDejarText.SetActive(false);

        AlTerminarInspeccion();
    }

    public Vector3 GetRotacionInicial() => rotacionInicial;
    public float GetDistanciaCamara() => distanciaCamara;
}