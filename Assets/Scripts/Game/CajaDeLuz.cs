using UnityEngine;

public class CajaDeLuz : MonoBehaviour
{
    [Header("Configuración de Luces")]
    [SerializeField] private GameObject[] lucesGarage;

    [Header("Obstáculo de Oscuridad")]
    [Tooltip("Arrastrá acá el Cubo Negro que bloquea el paso")]
    [SerializeField] private GameObject cuboBloqueador;

    public bool YaSeActivo { get; private set; } = false;

    void Start()
    {
     
        if (cuboBloqueador != null)
        {
            cuboBloqueador.SetActive(true);
        }
        ApagarLuces();
    }

    public void EncenderEnergia()
    {
        if (YaSeActivo) return;
        YaSeActivo = true;

       
        if (cuboBloqueador != null)
        {
            Destroy(cuboBloqueador);
        }

        // Encendemos tus focos reales del techo
        if (lucesGarage != null && lucesGarage.Length > 0)
        {
            foreach (GameObject luz in lucesGarage)
            {
                if (luz != null) luz.SetActive(true);
            }
            Debug.Log("¡Fusible colocado! Cubo eliminado y garage abierto.");
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
