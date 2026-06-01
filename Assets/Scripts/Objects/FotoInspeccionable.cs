using UnityEngine;

public class FotoInspeccionable : ObjetoInspeccionable
{
    [Header("Foto")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [TextArea]
    [SerializeField] private string mensajePista;

    [Header("Activación de Eventos")]
    [SerializeField] private GameObject monstruoAActivar;

    protected override void AlInspeccionar()
    {
        // 1. Mostrar el panel de la pista (se mantiene como antes)
        if (objectivePanel != null && !string.IsNullOrEmpty(mensajePista))
        {
            objectivePanel.Show(mensajePista);
        }

        // 2. Reconocer si es la foto del monstruo
        // Si este campo NO está vacío, significa que ES la foto elegida
        if (monstruoAActivar != null)
        {
            monstruoAActivar.SetActive(true); // Activa al enemigo en la escena
        }
    }
}