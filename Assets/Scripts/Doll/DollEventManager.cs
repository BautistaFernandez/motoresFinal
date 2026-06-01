using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DollEventManager : MonoBehaviour
{
    [Header("Configuración Doll")]
    private float waitingTime = 1f;
    [SerializeField] private DemonDoll demonDoll;

    [Header("Sonidos")]
    [SerializeField] private AudioSource risaAudio;
    [SerializeField] private AudioSource fondoAudio;

    [Header("Temporizador (Pasado a 10 Minutos)")]
    public float tiempoRestante = 600f; 
    private bool contadorActivo = false;
    [SerializeField] private TextMeshProUGUI textoReloj;
    [SerializeField] private float duracionHUD = 10f;
    [SerializeField] private List<TextMeshPro> textosRelojTVs;
    private string escenaGameOver = "GameOverScene";

    [Header("UI Alerta")]
    [SerializeField] private TextMeshProUGUI mensajeAlerta;

    void Start()
    {
       
        if (demonDoll != null) demonDoll.gameObject.SetActive(false);
        if (mensajeAlerta != null) mensajeAlerta.gameObject.SetActive(false);
        if (textoReloj != null) textoReloj.gameObject.SetActive(false);
    }

    
    public void GatillarInicioRealDelJuego()
    {
        if (demonDoll != null)
        {
            demonDoll.gameObject.SetActive(true);
            if (fondoAudio != null) fondoAudio.Play();
            StartCoroutine(BucleRisa());
        }

        
        IniciarContador();
    }

    private IEnumerator BucleRisa()
    {
        while (true)
        {
            if (risaAudio != null) risaAudio.Play();
            yield return new WaitForSecondsRealtime(10f);
        }
    }

    public void IniciarContador()
    {
        if (!contadorActivo)
        {
            contadorActivo = true;
            Time.timeScale = 1f;

            if (textoReloj != null) textoReloj.gameObject.SetActive(true);

            StartCoroutine(MostrarCartelEscapa());
            StartCoroutine(OcultarRelojHUD());
            Debug.Log("Contador de 10 min iniciado de forma oficial.");
        }
    }

    private IEnumerator OcultarRelojHUD()
    {
        yield return new WaitForSecondsRealtime(duracionHUD);
        if (textoReloj != null) textoReloj.gameObject.SetActive(false);
    }

    private IEnumerator MostrarCartelEscapa()
    {
        if (mensajeAlerta != null)
        {
            mensajeAlerta.text = "¡ESCAPA O MUERE! \nBusca las pistas en la casa";
            mensajeAlerta.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(4f);
            mensajeAlerta.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (contadorActivo)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.unscaledDeltaTime;
                ActualizarReloj();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1f;
                SceneManager.LoadScene(escenaGameOver);
            }
        }
    }

    void ActualizarReloj()
    {
        if (textoReloj != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoReloj.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            string textoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

            if (textoReloj != null) textoReloj.text = textoFormateado;

            if (textosRelojTVs != null)
            {
                foreach (TextMeshPro tv in textosRelojTVs)
                {
                    if (tv != null) tv.text = textoFormateado;
                }
            }
        }
    }
}