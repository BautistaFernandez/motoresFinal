using UnityEngine;

public abstract class ObjetoInspeccionable : MonoBehaviour
{
    [Header("Inspección")]
    [SerializeField] protected float distanciaCamara = 0.5f;
    [SerializeField] protected Vector3 rotacionInicial = Vector3.zero;

    public void Inspeccionar()
    {
        InspectorManager.Instance.IniciarInspeccion(this);
        AlInspeccionar();
    }

    public void TerminarInspeccion()
    {
        AlTerminarInspeccion();
    }

    public float GetDistanciaCamara() => distanciaCamara;
    public Vector3 GetRotacionInicial() => rotacionInicial;

    protected abstract void AlInspeccionar();
    protected virtual void AlTerminarInspeccion() { }
}
