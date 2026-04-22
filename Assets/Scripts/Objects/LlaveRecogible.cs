using UnityEngine;

public class LlaveRecogible : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribí el Tag de la puerta que abre (FinalDoor o FinalDoor2)")]
    public string tagPuertaAabrir;

    [Header("Objective UI")]
    [SerializeField] private ObjectivePanel objectivePanel;

    public void Recoger()
    {
        GameObject[] puertas = GameObject.FindGameObjectsWithTag(tagPuertaAabrir);

        foreach (GameObject obj in puertas)
        {
            var scriptPuerta = obj.GetComponent<ControladorPuerta>();
            if (scriptPuerta != null) scriptPuerta.tieneLlave = true;
        }

        if (tagPuertaAabrir == "FinalDoor")
        {
            objectivePanel.Show("Llave del garage encontrada. Descifra el mensaje de la carta");
        }
        else if (tagPuertaAabrir == "FinalDoor2")
        {
            objectivePanel.Show("Huye de la casa");
        }

        Destroy(gameObject);
    }
}