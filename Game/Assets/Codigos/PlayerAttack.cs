using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Hit settings")]
    public Transform hitOrigin;          // Punto desde donde contamos el alcance (ej. un empty en el puño)
    public float hitRadius = 0.7f;       // Radio del hit (ajusta según animación)
    public float hitOffset = 0.5f;       // Offset adelante desde hitOrigin (opcional)
    public LayerMask enemyLayer;         // Layer que identificará enemigos
    public int damage = 20;              // Daño base por golpe

    // (opcional) evitar golpear varias veces al mismo enemigo en el mismo golpe
    private HashSet<Collider> alreadyHit = new HashSet<Collider>();

    // Método que llamará el Animation Event
    // Si tu Animation Event pasa un parámetro float, puedes crear otra firma con float
    public void ApplyPunchDamage()
    {
        // Limpiar lista de ya golpeados para esta ventana de golpe
        alreadyHit.Clear();

        Vector3 origin = hitOrigin != null ? hitOrigin.position : transform.position;
        Vector3 center = origin + transform.forward * hitOffset;

        // OverlapSphere que devuelve todos los colliders en ese radio dentro del enemyLayer
        Collider[] hits = Physics.OverlapSphere(center, hitRadius, enemyLayer, QueryTriggerInteraction.Collide);

        foreach (var col in hits)
        {
            if (col == null) continue;

            if (alreadyHit.Contains(col)) continue;
            alreadyHit.Add(col);

            EnemyHealth enemy = col.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Golpeó a: " + col.name + " por " + damage);
            }
            else
            {
                // Si el collider está en un hijo del enemigo (ej. hitbox), buscamos componente en parents
                enemy = col.GetComponentInParent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("Golpeó (parent) a: " + col.name + " por " + damage);
                }
            }
        }

        // Puedes instanciar efectos aquí:
        // Instantiate(impactVFX, center, Quaternion.identity);
    }

    // Para depurar: dibuja el sphere de impacto en la escena (solo en editor)
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 origin = hitOrigin != null ? hitOrigin.position : transform.position;
            Vector3 center = origin + transform.forward * hitOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, hitRadius);
        }
    }
}

