using UnityEngine;
using UnityEngine.InputSystem;

public class FistAttack : MonoBehaviour
{
    [Header("Fist Settings")]
    [SerializeField] private Transform fistPoint;     // Punto desde donde se detecta el golpe
    [SerializeField] private float fistRate = 0.4f;   // Tiempo entre golpes
    private float fistRateTime;

    [Header("Input")]
    [SerializeField] private InputActionReference fistAction; // Botón del golpe

    [Header("Damage Settings")]
    [SerializeField] private float fistDamage = 15f;  // Daño
    [SerializeField] private float hitRadius = 0.7f;  // Radio del golpe
    [SerializeField] private LayerMask enemyLayer;     // Layer del enemigo


    private void OnEnable()
    {
        fistAction.action.performed += OnFist;
        fistAction.action.Enable();
    }

    private void OnDisable()
    {
        fistAction.action.performed -= OnFist;
        fistAction.action.Disable();
    }


    private void OnFist(InputAction.CallbackContext ctx)
    {
        // Control de cadencia
        if (Time.time < fistRateTime) return;

        fistRateTime = Time.time + fistRate;

        DoPunchDamage();
    }


    private void DoPunchDamage()
    {
        Vector3 center = fistPoint.position;

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


    private void OnDrawGizmosSelected()
    {
        if (fistPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fistPoint.position, hitRadius);
    }
}


