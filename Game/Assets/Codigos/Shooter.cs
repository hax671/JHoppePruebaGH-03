using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] private Transform Point;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float ShootForce = 20f;
    [SerializeField] private float ShootRate = 0.2f;
    private float ShootRateTime;

    [Header("Input")]
    [SerializeField] private InputActionReference ShootAction;

    [Header("Bullet Settings")]
    [SerializeField] private float bulletLifeTime = 5f;

    [Header("Damage Settings")]
    [SerializeField] private float bulletDamage = 20f;   // <-- Daño configurable

    private void OnEnable()
    {
        ShootAction.action.performed += OnShoot;
        ShootAction.action.Enable();
    }

    private void OnDisable()
    {
        ShootAction.action.performed -= OnShoot;
        ShootAction.action.Disable();
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (Time.time < ShootRateTime) return;

        ShootRateTime = Time.time + ShootRate;

        // Instanciar la bala
        GameObject newBullet = Instantiate(bullet, Point.position, Point.rotation);

        // Establecer daño en la bala
        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }

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





