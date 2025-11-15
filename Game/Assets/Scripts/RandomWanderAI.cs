using UnityEngine;
using UnityEngine.AI;

public class RandomWanderAI : MonoBehaviour
{
    [Header("NavMesh & Animator")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private string walkParam = "isWalking"; // parámetro Bool en el Animator

    [Header("Wander settings")]
    [SerializeField] private float wanderRadius = 10f;      // radio para elegir puntos aleatorios
    [SerializeField] private float wanderInterval = 0f;     // si >0, fuerza elegir nuevo destino cada x segundos
    [SerializeField] private float arriveThreshold = 0.4f;  // distancia para considerar que llegó

    [Header("Idle timing")]
    [SerializeField] private float idleMin = 1.0f; // tiempo mínimo quieto
    [SerializeField] private float idleMax = 4.0f; // tiempo máximo quieto

    private float wanderTimer = 0f;
    private bool isIdle = false;

    private void Reset()
    {
        // intentamos auto-asignar componentes si se añade desde el editor
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        ChooseNewDestination();
    }

    private void Update()
    {
        if (agent == null || animator == null) return;

        // Si está en idle (quieto) no revisar camino
        if (isIdle)
        {
            animator.SetBool(walkParam, false);
            return;
        }

        // Si hemos definido wanderInterval, forzamos nuevo destino
        if (wanderInterval > 0f)
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderInterval)
            {
                wanderTimer = 0f;
                ChooseNewDestination();
            }
        }

        // Actualizar animador según movimiento del agente
        bool walking = agent.velocity.sqrMagnitude > 0.01f && agent.remainingDistance > arriveThreshold;
        animator.SetBool(walkParam, walking);

        // Si llegó al destino -> quedar idle por un tiempo aleatorio y luego continuar
        if (!agent.pathPending && agent.remainingDistance <= arriveThreshold && !isIdle)
        {
            StartCoroutine(DoIdleThenContinue());
        }
    }

    private void ChooseNewDestination()
    {
        // intenta encontrar un punto válido sobre el NavMesh dentro del radio
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // fallback: intenta otra vez con dirección aleatoria más cercana
            agent.SetDestination(transform.position);
        }

        wanderTimer = 0f;
    }

    private System.Collections.IEnumerator DoIdleThenContinue()
    {
        isIdle = true;
        animator.SetBool(walkParam, false);

        float idleTime = Random.Range(idleMin, idleMax);
        yield return new WaitForSeconds(idleTime);

        isIdle = false;
        ChooseNewDestination();
    }
}

