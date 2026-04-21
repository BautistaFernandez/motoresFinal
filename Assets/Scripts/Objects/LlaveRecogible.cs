using UnityEngine;

public class LlaveRecogible : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribí el Tag de la puerta que abre (FinalDoor o FinalDoor2)")]
    public string tagPuertaAabrir;

    public void Recoger()
    {
        GameObject[] puertas = GameObject.FindGameObjectsWithTag(tagPuertaAabrir);

        foreach (GameObject obj in puertas)
        {
            var scriptPuerta = obj.GetComponent<ControladorPuerta>();
            if (scriptPuerta != null) scriptPuerta.tieneLlave = true;
        }

        Debug.Log("Llave recogida para: " + tagPuertaAabrir);
        Destroy(gameObject);
    }
}