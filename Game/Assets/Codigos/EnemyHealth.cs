using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public float maxHealth = 100f;   // Vida máxima configurable desde Unity
    [HideInInspector] public float currentHealth; // Vida actual (solo lectura en Inspector)

    private void Start()
    {
        currentHealth = maxHealth;   // La vida inicia al máximo
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} recibió {damage} daño. Vida actual: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }

    // Opcional: Método para restaurar vida si quieres
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}


