using UnityEngine;

public class LlaveRecogible : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private KeyLock lockAabrir;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [TextArea]
    [SerializeField] private string mensajeObjetivo;

    public void Recoger()
    {
        if (lockAabrir != null) lockAabrir.Unlock();
        if (objectivePanel != null) objectivePanel.Show(mensajeObjetivo);

        Destroy(gameObject);
    }
}

