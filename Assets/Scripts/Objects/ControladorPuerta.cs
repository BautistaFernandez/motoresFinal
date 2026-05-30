using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ControladorPuerta : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidad = 3f;

    [Header("Doors Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;

    [Header("UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    [Header("Escape")]
    [SerializeField] private bool esPuertaDeEscape = false;
    [SerializeField] private string escenaVictoria = "WinScene";
    [SerializeField] private float delayVictoria = 1.5f;

    [Header("Eventos")]
    [SerializeField] private UnityEvent onDoorOpened;

    private bool estaAbierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;
    private ILock[] locks;

    private void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0, 0, anguloApertura);

        locks = GetComponents<ILock>();
    }

    private void Update()
    {
        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidad);
    }

    public void IntentarAbrir()
    {
        foreach (ILock lockComp in locks)
        {
            if (!lockComp.IsUnlocked)
            {
                if (objectivePanel != null) objectivePanel.Show(lockComp.GetLockMessage());
                return;
            }
        }

        Accionar();

        if (esPuertaDeEscape && estaAbierta)
        {
            Invoke(nameof(IrAVictoria), delayVictoria);
        }
    }

    private void Accionar()
    {
        estaAbierta = !estaAbierta;
        if (audioSource != null) audioSource.PlayOneShot(estaAbierta ? openDoor : closeDoor);

        if (estaAbierta) onDoorOpened?.Invoke();
    }

    private void IrAVictoria()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(escenaVictoria);
    }
}