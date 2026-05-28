using UnityEngine;

public class CajaDeLuz : MonoBehaviour
{
    [Header("Configuración de Luces")]
    [Tooltip("Arrastrá acá las luces individuales del garage (Focus_01, Focus_01.002, etc.)")]
    [SerializeField] private GameObject[] lucesGarage;

    
    public bool YaSeActivo { get; private set; } = false;

    void Start()
    {
        if (lucesGarage != null)
        {
            ApagarLuces();
        }
    }

    public void EncenderEnergia()
    {
        if (YaSeActivo) return;

        YaSeActivo = true;

        if (lucesGarage != null && lucesGarage.Length > 0)
        {
            foreach (GameObject luz in lucesGarage)
            {
                if (luz != null) luz.SetActive(true);
            }
            Debug.Log("¡Energía restablecida!");
        }
    }

    private void ApagarLuces()
    {
        if (lucesGarage != null)
        {
            foreach (GameObject luz in lucesGarage)
            {
                if (luz != null) luz.SetActive(false);
            }
        }
    }
}