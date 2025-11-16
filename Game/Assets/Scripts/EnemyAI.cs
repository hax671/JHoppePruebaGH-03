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

    [Header("Patrulla Aleatoria")]
    public float wanderRadius = 10f;
    public float idleMin = 1f;
    public float idleMax = 4f;
    public float arriveThreshold = 0.4f;

    [Header("Detección")]
    public float visionRange = 12f;
    public float attackRange = 1.5f;

    // << NUEVO >>
    [Header("Velocidades")]
    public float normalSpeed = 2f;
    public float chaseSpeed = 4f;

    private Vector3 patrolPoint;
    private bool isIdle = false;
    private bool isChasing = false;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        // velocidad normal al iniciar
        agent.speed = normalSpeed;

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (!isChasing)
        {
            if (CanSeePlayer())
            {
                isChasing = true;

                // << AUMENTAR VELOCIDAD AL DETECTAR >>
                agent.speed = chaseSpeed;

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

    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= visionRange;
    }

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

        // si pierde al jugador
        if (distance > visionRange + 2f)
        {
            isChasing = false;

            // << VOLVER A VELOCIDAD NORMAL >>
            agent.speed = normalSpeed;

            ChooseNewPatrolPoint();
        }
    }

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






