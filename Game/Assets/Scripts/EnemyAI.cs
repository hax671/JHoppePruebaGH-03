using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Jugador")]
    public Transform player;

    [Header("Parámetros de Animación")]
    public string walkParam = "isWalking";
    public string attackTrigger = "MeleeAttack_0";
    public string walkSpeedParam = "walkSpeed"; // <-- nuevo parámetro

    [Header("Velocidades")]
    public float walkSpeed = 1.5f;   // velocidad normal
    public float chaseSpeed = 4f;    // velocidad cuando persigue

    public float walkAnimSpeed = 1f;     // velocidad animación normal
    public float chaseAnimSpeed = 1.8f;  // velocidad animación persiguiendo

    [Header("Patrulla Aleatoria")]
    public float wanderRadius = 10f;
    public float idleMin = 1f;
    public float idleMax = 4f;
    public float arriveThreshold = 0.4f;

    [Header("Detección")]
    public float visionRange = 12f;
    public float attackRange = 1.5f;

    private Vector3 patrolPoint;
    private bool isIdle = false;
    private bool isChasing = false;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        animator.SetFloat(walkSpeedParam, walkAnimSpeed);

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (!isChasing)
        {
            if (CanSeePlayer())
            {
                isChasing = true;
                agent.speed = chaseSpeed;                                // <-- velocidad navmesh al detectar
                animator.SetFloat(walkSpeedParam, chaseAnimSpeed);       // <-- subir velocidad animación
                animator.SetBool(walkParam, true);
            }
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // ------------------------ DETECCIÓN ------------------------

    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= visionRange;
    }

    // ------------------------ PERSECUCIÓN ------------------------

    void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        agent.SetDestination(player.position);
        animator.SetBool(walkParam, true);

        if (distance <= attackRange)
        {
            agent.ResetPath();
            animator.SetBool(walkParam, false);
            animator.SetTrigger(attackTrigger);
        }

        if (distance > visionRange + 2f)
        {
            isChasing = false;

            agent.speed = walkSpeed;                                // <-- regresar velocidad normal
            animator.SetFloat(walkSpeedParam, walkAnimSpeed);       // <-- regresar animación normal

            ChooseNewPatrolPoint();
        }
    }

    // ------------------------ PATRULLA ------------------------

    void Patrol()
    {
        if (isIdle)
        {
            animator.SetBool(walkParam, false);
            return;
        }

        animator.SetBool(walkParam, true);
        agent.SetDestination(patrolPoint);

        if (!agent.pathPending && agent.remainingDistance <= arriveThreshold)
        {
            StartCoroutine(IdleThenWalk());
        }
    }

    System.Collections.IEnumerator IdleThenWalk()
    {
        isIdle = true;
        animator.SetBool(walkParam, false);

        float wait = Random.Range(idleMin, idleMax);
        yield return new WaitForSeconds(wait);

        isIdle = false;
        ChooseNewPatrolPoint();
    }

    void ChooseNewPatrolPoint()
    {
        Vector3 random = Random.insideUnitSphere * wanderRadius;
        random += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(random, out hit, wanderRadius, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
        }
        else
        {
            patrolPoint = transform.position;
        }
    }
}







