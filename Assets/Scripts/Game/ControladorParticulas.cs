using UnityEngine;

public class ControladorParticulas : MonoBehaviour
{
    private ParticleSystem particulas;

    private void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        CajaDeLuz.OnEnergiaRestaurada += ActivarParticulas;
    }

    private void OnDisable()
    {
        CajaDeLuz.OnEnergiaRestaurada -= ActivarParticulas;
    }

    private void ActivarParticulas()
    {
        if (particulas != null)
        {
            particulas.Play();
        }
    }
}
