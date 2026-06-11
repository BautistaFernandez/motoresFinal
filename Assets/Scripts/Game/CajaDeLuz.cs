using UnityEngine;
using System;

public class CajaDeLuz : MonoBehaviour
{
    [Header("Configuración de Luces")]
    [SerializeField] private GameObject[] lucesGarage;

    [Header("Obstáculo de Oscuridad")]
    [SerializeField] private GameObject cuboBloqueador;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;


    public bool YaSeActivo { get; private set; } = false;

    public static event Action OnEnergiaRestaurada;

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
        }

        if (objectivePanel != null) objectivePanel.Show("Vuelve al garage y encuentra la llave");

        OnEnergiaRestaurada?.Invoke();
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
