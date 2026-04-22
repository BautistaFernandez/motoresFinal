using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoor, closeDoor;
    [SerializeField] private ObjectivePanel objectivePanel;

    [Header("Estado")]
    public bool tieneLlave = false;
    private bool estaAbierta = false;
    private Quaternion rotacionCerrada, rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, 0, anguloApertura);
    }

    void Update()
    {
       
        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

   
    public void IntentarAbrir()
    {
        if (CompareTag("FinalDoor"))
        {
            if (tieneLlave) Accionar();
            else if (objectivePanel != null) objectivePanel.Show("Encuentra la llave del garage");
        }
        else if (CompareTag("FinalDoor2"))
        {
            if (tieneLlave)
            {
                Accionar();
                if (estaAbierta) Invoke("IrAVictoria", 1.5f);
            }
            else if (objectivePanel != null) objectivePanel.Show("Busca la llave de escape");
        }
        else
        {
            Accionar(); 
        }
    }

    private void Accionar()
    {
        estaAbierta = !estaAbierta;
        if (audioSource != null) audioSource.PlayOneShot(estaAbierta ? openDoor : closeDoor);
    }

    private void IrAVictoria()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("WinScene");
    }
}