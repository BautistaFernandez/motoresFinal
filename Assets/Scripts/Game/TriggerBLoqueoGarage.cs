using UnityEngine;

public class TriggerBloqueoGarage : MonoBehaviour
{
    [Header("Referencias")]
    public ObjectivePanel panelObjetivos;

    [TextArea]
    public string mensajeFallo = "Es tan espesa la oscuridad que la linterna no alumbra, voy a tener que ir a la caja de fusibles para restablecer la energía.";

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            if (panelObjetivos != null)
            {
                panelObjetivos.Show(mensajeFallo);
            }
        }
    }
}