using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("Shoot Settings")]
    public Transform Point;             // Punto desde donde se instancia la bala
    public GameObject bullet;           // Prefab de la bala
    public float ShootForce = 20f;      // Fuerza de la bala
    public float ShootRate = 0.2f;      // Tiempo mínimo entre disparos
    private float ShootRateTime;        // Temporizador interno

    [Header("Input")]
    public InputActionReference ShootAction;   // <-- Tú eliges el botón en el inspector

    [Header("Bullet Settings")]
    public float bulletLifeTime = 5f;   // Tiempo antes de destruir la bala


    private void OnEnable()
    {
        ShootAction.action.performed += OnShoot;   // Se ejecuta solo al presionar
        ShootAction.action.Enable();
    }

    private void OnDisable()
    {
        ShootAction.action.performed -= OnShoot;
        ShootAction.action.Disable();
    }


    private void OnShoot(InputAction.CallbackContext ctx)
    {
        // Control de cadencia de disparo
        if (Time.time < ShootRateTime) return;

        ShootRateTime = Time.time + ShootRate;

        // Instanciar la bala
        GameObject newBullet = Instantiate(bullet, Point.position, Point.rotation);

        // Aplicar fuerza
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Point.forward * ShootForce, ForceMode.Impulse);
        }

        // Destruir después de X segundos
        Destroy(newBullet, bulletLifeTime);
    }
}

