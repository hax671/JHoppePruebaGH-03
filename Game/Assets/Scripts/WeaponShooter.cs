using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Transform spawnPoint;               // Dónde aparece la bala
    public GameObject bullet;                  // Prefab de la bala

    [Header("Disparo")]
    public float shootForce = 20f;             // Fuerza de salida
    public float shootRate = 0.3f;             // Tiempo entre disparos

    private float shootRateTime = 0f;          // Temporizador interno

    [Header("Input System")]
    public InputActionReference shootAction;   // Aquí asignas el botón

    private void OnEnable()
    {
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
    }

    void Update()
    {
        // Actualizar el temporizador
        if (shootRateTime > 0)
            shootRateTime -= Time.deltaTime;

        // Revisar si el botón se presiona (performed)
        if (shootAction.action.IsPressed() && shootRateTime <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instanciar la bala
        GameObject newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);

        // Aplicar fuerza
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.AddForce(spawnPoint.forward * shootForce, ForceMode.Impulse);

        // Reiniciar temporizador
        shootRateTime = shootRate;
    }
}

