using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Transform spawnPoint;         // Dónde aparece la bala
    public GameObject bullet;            // Prefab de la bala

    [Header("Disparo")]
    public float shootForce = 20f;       // Fuerza de salida
    public float shootRate = 0.3f;       // Tiempo entre disparos

    private float shootRateTime = 0f;    // Contador interno

    void Update()
    {
        // Actualizar el temporizador
        if (shootRateTime > 0)
            shootRateTime -= Time.deltaTime;

        // Disparar con click izquierdo o el input que quieras
        if (Input.GetButton("Fire1") && shootRateTime <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Crear la bala en el spawnPoint
        GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        // Agregar fuerza a la bala
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.AddForce(spawnPoint.forward * shootForce, ForceMode.Impulse);

        // Reiniciar el tiempo del disparo
        shootRateTime = shootRate;
    }
}


