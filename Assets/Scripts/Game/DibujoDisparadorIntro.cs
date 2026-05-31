using UnityEngine;

public class DibujoDisparadorIntro : MonoBehaviour
{
    private bool yaSeUso = false;

    public void ActivarScreamerDeIntro()
    {
        if (yaSeUso) return;
        yaSeUso = true;

        Debug.Log("DibujoDisparadorIntro: Avisando al IntroManager...");

        // Llamamos directamente a la instancia estática segura
        if (IntroManager.Instance != null)
        {
            IntroManager.Instance.DibujoDeCocinaInspeccionado();
        }
    }
}