using UnityEngine;
using UnityEngine.AI;

public class EnemyRoute : MonoBehaviour
{
    private NavMeshAgent agente;

    [Header("Configuración de Ruta")]
    [SerializeField] private Transform puntoB;

    [Header("Detección por cámara")]
    [SerializeField] private Camera playerCamera;

    private bool yaArrancoCaminar = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (puntoB == null)
        {
            Debug.LogWarning($"No asignaste el Punto B en el enemigo {gameObject.name}");
        }
    }

    void Update()
    {
        if (!yaArrancoCaminar)
        {
            if (EnemigoVistoPorPlayer())
            {
                yaArrancoCaminar = true;
                if (puntoB != null) agente.SetDestination(puntoB.position);
            }
            return;
        }

        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            if (!agente.hasPath || agente.velocity.sqrMagnitude == 0f)
            {
                DesaparecerEnemigo();
            }
        }
    }

    private bool EnemigoVistoPorPlayer()
    {
        if (playerCamera == null) return false;

        Vector3 viewportPos = playerCamera.WorldToViewportPoint(transform.position);
        return viewportPos.z > 0 &&
               viewportPos.x > 0 && viewportPos.x < 1 &&
               viewportPos.y > 0 && viewportPos.y < 1;
    }

    private void DesaparecerEnemigo()
    {
        Destroy(gameObject);
    }
}