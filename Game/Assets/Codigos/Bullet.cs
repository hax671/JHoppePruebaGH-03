using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;   // Daño que hará la bala

    private void OnCollisionEnter(Collision collision)
    {
        // Intentamos obtener el script de salud del enemigo
        EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // La bala se destruye al impactar algo
        Destroy(gameObject);
    }
}

