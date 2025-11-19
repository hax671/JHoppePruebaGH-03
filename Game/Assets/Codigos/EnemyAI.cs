using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [Header("Jugador")]
    [SerializeField] private Transform player;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip detectClip;
    [SerializeField] private AudioClip attackClip;        // sonido ataque principal
    [SerializeField] private AudioClip attackExtraClip;   // sonido extra al atacar

    [Header("Attack Sound Cooldown")]
    [SerializeField] private float attackSoundCooldown = 0.6f;
    private float attackSoundTimer = 0f;

    [Header("Parámetros de Animación")]
    [SerializeField] private string walkParam = "isWalking";
    [SerializeField] private string attackTrigger = "MeleeAttack_0";
    [SerializeField] private string walkSpeedParam = "walkSpeed";

    [Header("Velocidades")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float chaseSpeed = 4f;

    [SerializeField] private float walkAnimSpeed = 1f;
    [SerializeField] private float chaseAnimSpeed = 1.8f;

    [Header("Patrulla Aleatoria")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float idleMin = 1f;
    [SerializeField] private float idleMax = 4f;
    [SerializeField] private float arriveThreshold = 0.4f;

    [Header("Detección")]
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private float attackRange = 1.5f;

    private Vector3 patrolPoint;
    private bool isIdle = false;
    private bool isChasing = false;
    private bool detectSoundPlayed = false;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        agent.speed = walkSpeed;
        animator.SetFloat(walkSpeedParam, walkAnimSpeed);

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        // actualizar cooldown del ataque
        if (attackSoundTimer > 0)
            attackSoundTimer -= Time.deltaTime;

        if (!isChasing)
        {
            if (CanSeePlayer())
            {
                isChasing = true;

                if (!detectSoundPlayed && detectClip != null)
                {
                    audioSource.PlayOneShot(detectClip);
                    detectSoundPlayed = true;
                }

                agent.speed = chaseSpeed;
                animator.SetFloat(walkSpeedParam, chaseAnimSpeed);
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

        if (distance > visionRange + 2f)
        {
            isChasing = false;
            detectSoundPlayed = false;

            agent.speed = walkSpeed;
            animator.SetFloat(walkSpeedParam, walkAnimSpeed);

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

    // --------------------------------------------------------
    // SONIDO PRINCIPAL DE ATAQUE (ANIMATION EVENT)
    // --------------------------------------------------------
    public void PlayAttackSound()
    {
        if (attackClip == null) return;
        if (attackSoundTimer > 0) return;

        audioSource.PlayOneShot(attackClip);
        attackSoundTimer = attackSoundCooldown;
    }

    // --------------------------------------------------------
    // SONIDO EXTRA DURANTE EL ATAQUE (ANIMATION EVENT)
    // --------------------------------------------------------
    public void PlayAttackExtraSound()
    {
        if (attackExtraClip != null)
            audioSource.PlayOneShot(attackExtraClip);
    }
}















