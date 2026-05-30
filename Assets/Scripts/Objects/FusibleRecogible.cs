using UnityEngine;


public class FusibleRecogible : MonoBehaviour
{
    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    public void Recoger()
    {
        objectivePanel.Show("Coloca el fusible en el panel eléctrico");
        Destroy(gameObject);
    }
}