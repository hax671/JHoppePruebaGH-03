using UnityEngine;
using UnityEngine.InputSystem;

public class XRShootController : MonoBehaviour
{
    [Header("References")]
    public Transform SpawnPoint;          // Punto donde se instanciará la bala
    public GameObject bullet;             // Prefab de la bala

    [Header("Shoot Settings")]
    public float ShootForce = 20f;        // Fuerza con la que se dispara la bala
    public float ShootRate = 0.5f;        // Tiempo mínimo entre disparos
    private float ShootRateTime = 0f;     // Contador del tiempo desde el último disparo
    public float BulletLifeTime = 3f;     // Tiempo para destruir la bala

    [Header("Input")]
    public InputActionReference shootActionReference; // Acción de disparo asignable

    private void OnEnable()
    {
        if (shootActionReference != null)
            shootActionReference.action.performed += ShootPerformed;
    }

    private void OnDisable()
    {
        if (shootActionReference != null)
            shootActionReference.action.performed -= ShootPerformed;
    }

    private void Update()
    {
        ShootRateTime += Time.deltaTime;
    }

    private void ShootPerformed(InputAction.CallbackContext context)
    {
        // Solo disparamos cuando se presiona el botón una vez
        if (ShootRateTime >= ShootRate)
        {
            ShootRateTime = 0f;

            // Instanciar la bala en SpawnPoint
            GameObject newBullet = Instantiate(bullet, SpawnPoint.position, SpawnPoint.rotation);

            // Aplicar fuerza
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(SpawnPoint.forward * ShootForce, ForceMode.Impulse);
            }

            // Destruir la bala después de X tiempo
            Destroy(newBullet, BulletLifeTime);
        }
    }
}



