using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    [Header("Hit Color Settings")]
    public float hitTime = 0.2f;       // Tiempo que se mantiene rojo

    private Renderer[] renderers;      // Todos los renderers del enemigo
    private Color[] originalColors;    // Guardamos todos los colores originales


    private void Start()
    {
        currentHealth = maxHealth;

        // Obtener TODOS los renderers del enemigo (incluyendo hijos)
        renderers = GetComponentsInChildren<Renderer>();

        // Guardar los colores originales
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // 🔥 Mostrar vida en la consola
        Debug.Log($"{gameObject.name} vida actual: {currentHealth}");

        // Cambiar a rojo temporalmente
        StartCoroutine(HitEffect());

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private IEnumerator HitEffect()
    {
        // Cambiar TODOS a rojo
        foreach (Renderer r in renderers)
        {
            r.material.color = Color.red;
        }

        // Esperar X segundos
        yield return new WaitForSeconds(hitTime);

        // Restaurar colores originales
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(gameObject);
    }
}





