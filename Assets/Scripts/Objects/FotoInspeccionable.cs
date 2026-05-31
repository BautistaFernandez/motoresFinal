using UnityEngine;

public class FotoInspeccionable : ObjetoInspeccionable
{
    [Header("Foto")]
    [SerializeField] private ObjectivePanel objectivePanel;
    [TextArea]
    [SerializeField] private string mensajePista;

    protected override void AlInspeccionar()
    {
        if (objectivePanel != null && !string.IsNullOrEmpty(mensajePista))
        {
            objectivePanel.Show(mensajePista);
        }
    }
}