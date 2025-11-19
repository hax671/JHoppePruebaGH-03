using UnityEngine;
using UnityEngine.InputSystem;

public class FistAttack : MonoBehaviour
{
    [Header("Fist Settings")]
    public Transform FistPoint;        // Punto desde donde se hace el golpe (punta del puño)
    public float FistRate = 0.4f;      // Tiempo mínimo entre golpes
    private float FistRateTime;

    [Header("Input")]
    public InputActionReference FistAction; // Botón del golpe

    [Header("Damage Settings")]
    public float fistDamage = 15f;     // Daño del golpe
    public float hitRadius = 0.7f;     // Radio del golpe para detectar enemigos
    public LayerMask enemyLayer;       // Layer para enemigos (si no tienes, puedes usar Default)



    private void OnEnable()
    {
        FistAction.action.performed += OnFist;
        FistAction.action.Enable();
    }

    private void OnDisable()
    {
        FistAction.action.performed -= OnFist;
        FistAction.action.Disable();
    }


    private void OnFist(InputAction.CallbackContext ctx)
    {
        // Controla cada cuánto puedes golpear
        if (Time.time < FistRateTime) return;

        FistRateTime = Time.time + FistRate;

        // Hacer daño
        DoPunchDamage();
    }


    private void DoPunchDamage()
    {
        Vector3 center = FistPoint.position;

        // Detectamos enemigos en un radio alrededor del puño
        Collider[] hits = Physics.OverlapSphere(center, hitRadius, enemyLayer);

        foreach (Collider col in hits)
        {
            EnemyHealth enemy = col.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(fistDamage);
                Debug.Log("Golpeado: " + col.name);
            }
        }
    }


    // Para ver el área del golpe en la escena
    private void OnDrawGizmosSelected()
    {
        if (FistPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FistPoint.position, hitRadius);
    }
}

