using UnityEngine;
using UnityEngine.InputSystem;

public class XRRunAndShootController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Run Settings")]
    [SerializeField] private string runParameter = "Run";
    [SerializeField] private InputActionReference moveActionReference;

    [Header("Shoot Settings")]
    [SerializeField] private string shootTrigger = "shoot";
    [SerializeField] private InputActionReference shootActionReference;

    [Header("Shoot Cooldown")]
    [SerializeField] private float shootCooldown = 0.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource shootAudioSource;   // Sonido de disparo
    [SerializeField] private AudioSource runAudioSource;     // 🔊 Nuevo: sonido de correr

    private bool isShooting = false;
    private bool wasRunning = false; // Para detectar cambio de estado

    private void OnEnable()
    {
        moveActionReference.action.Enable();
        shootActionReference.action.Enable();
        shootActionReference.action.performed += OnShoot;
    }

    private void OnDisable()
    {
        moveActionReference.action.Disable();
        shootActionReference.action.Disable();
        shootActionReference.action.performed -= OnShoot;
    }

    private void Update()
    {
        Vector2 moveValue = moveActionReference.action.ReadValue<Vector2>();
        bool isRunning = moveValue.y > 0.5f;

        // Actualizar animación
        animator.SetBool(runParameter, isRunning);

        // ---------------------------
        // 🔊 CONTROL DEL SONIDO RUN
        // ---------------------------
        if (isRunning && !wasRunning)
        {
            // Empezó a correr → reproducir sonido
            if (runAudioSource != null && !runAudioSource.isPlaying)
                runAudioSource.Play();
        }
        else if (!isRunning && wasRunning)
        {
            // Dejó de correr → detener sonido
            if (runAudioSource != null)
                runAudioSource.Stop();
        }

        wasRunning = isRunning;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (isShooting) return;
        isShooting = true;

        // Animación
        animator.SetTrigger(shootTrigger);

        // Sonido del disparo
        if (shootAudioSource != null)
            shootAudioSource.Play();

        // Desbloquear disparo después del cooldown
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    private void ResetShoot()
    {
        isShooting = false;
    }
}







