using UnityEngine;

public class PickupLinterna : ObjetoInspeccionable
{
    [Header("Referencia del Apagón")]
    public GameObject luzPrincipal; 

    protected override void AlInspeccionar()
    {

        if (luzPrincipal != null)
        {
            luzPrincipal.SetActive(false);
        }
    }
}