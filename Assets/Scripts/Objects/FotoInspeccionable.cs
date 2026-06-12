using UnityEngine;

public class FotoInspeccionable : ObjetoInspeccionable
{
    [Header("Foto")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [TextArea]
    [SerializeField] private string mensajePista;

    [Header("Activación de Eventos")]
    [SerializeField] private GameObject monstruoAActivar;

    // ¡NUEVO! La conexión con el reloj
    [SerializeField] private ManagerApariciones managerApariciones;

    protected override void AlInspeccionar()
    {
        // 1. Mostrar el panel de la pista
        if (objectivePanel != null && !string.IsNullOrEmpty(mensajePista))
        {
            objectivePanel.Show(mensajePista);
        }

        // 2. Activar el evento visual de la foto
        if (monstruoAActivar != null)
        {
            monstruoAActivar.SetActive(true);
        }

        
        if (managerApariciones != null)
        {
            managerApariciones.EmpezarContadorEvil();
        }
    }
}