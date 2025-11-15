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
    public string walkParam = "isWalking";      // bool caminar
    public string attackTrigger = "MeleeAttack_0"; // trigger de ataque

    [Header("Patrulla Aleatoria")]
    public float wanderRadius = 10f;
    public float idleMin = 1f;
    public float idleMax = 4f;
    public float arriveThreshold = 0.4f;

    [Header("Detección")]
    public float visionRange = 12f;       // distancia para detectar al jugador (360°)
    public float attackRange = 1.5f;      // rango de ataque

    private Vector3 patrolPoint;
    private bool isIdle = false;
    private bool isChasing = false;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (!isChasing)
        {
            // si aún no persigue, revisar si el jugador entra en el radio de detección
            if (CanSeePlayer())
            {
                isChasing = true;
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

    // Detección 360° SOLO por distancia (sin ángulo, sin raycast)
    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Si entra en el radio, se detecta automáticamente
        return dist <= visionRange;
    }

    // ------------------------ PERSECUCIÓN ------------------------

    void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        agent.SetDestination(player.position);
        animator.SetBool(walkParam, true);

        // atacar si está cerca
        if (distance <= attackRange)
        {
            agent.ResetPath();
            animator.SetBool(walkParam, false);
            animator.SetTrigger(attackTrigger);
        }

        // Si el jugador se aleja demasiado, vuelve a patrullar
        if (distance > visionRange + 2f)
        {
            isChasing = false;
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

