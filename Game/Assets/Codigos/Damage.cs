using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    [SerializeField] private Transform player;         // Referencia al jugador
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private float damageDistance = 1.2f;  // Distancia para hacer daño

    private PlayerHealth playerHealth;

    private void Start()
    {
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= damageDistance)
        {
            StartCoroutine(DoDamage());
        }
    }

    private bool damaging = false;

    private IEnumerator DoDamage()
    {
        if (damaging) yield break; // evitar múltiples rutinas
        damaging = true;

        while (Vector3.Distance(transform.position, player.position) <= damageDistance)
        {
            playerHealth.TakeDamage(damagePerSecond * damageInterval);
            yield return new WaitForSeconds(damageInterval);
        }

        damaging = false;
    }
}


