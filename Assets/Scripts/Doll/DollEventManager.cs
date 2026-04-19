using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class DollEventManager : MonoBehaviour
{
    [Header("Configuración Doll")]
    private float waitingTime = 10f;
    [SerializeField] private DemonDoll demonDoll;
    public AudioSource risaAudio; 

    [Header("Temporizador")]
    public float tiempoRestante = 120f;
    private bool contadorActivo = false;
    public TextMeshProUGUI textoReloj;
    public string escenaGameOver = "GameOver";

    [Header("UI Alerta")]
    public TextMeshProUGUI mensajeAlerta;

    void Start()
    {
       
        if (demonDoll != null) demonDoll.gameObject.SetActive(false);

        
        if (mensajeAlerta != null) mensajeAlerta.gameObject.SetActive(false);
        if (textoReloj != null) textoReloj.gameObject.SetActive(false);

        StartCoroutine(timerAparision());
    }

    private IEnumerator timerAparision()
    {
        yield return new WaitForSeconds(waitingTime);
        if (demonDoll != null) demonDoll.gameObject.SetActive(true);

      
        if (risaAudio != null) risaAudio.Play();
    }

    public void IniciarContador()
    {
        if (!contadorActivo)
        {
            contadorActivo = true;

           
            if (textoReloj != null) textoReloj.gameObject.SetActive(true);
            StartCoroutine(MostrarCartelEscapa());

            Debug.Log("Evento iniciado: Contador y Alerta activados.");
        }
    }

    private IEnumerator MostrarCartelEscapa()
    {
        if (mensajeAlerta != null)
        {
            mensajeAlerta.text = "¡ESCAPA O MUERE !";
            mensajeAlerta.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            mensajeAlerta.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (contadorActivo)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarReloj();
            }
            else
            {
               
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
        }
    }
}