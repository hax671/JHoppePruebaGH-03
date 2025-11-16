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
    [SerializeField] private float shootCooldown = 0.5f; // 🔥 Controlas aquí el tiempo de bloqueo

    [Header("Audio Settings")]
    [SerializeField] private AudioSource shootAudioSource;

    private bool isShooting = false;

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
        animator.SetBool(runParameter, isRunning);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        // 🔒 Evitar repetir disparos
        if (isShooting) return;
        isShooting = true;

        // Animación
        animator.SetTrigger(shootTrigger);

        // Sonido
        if (shootAudioSource != null)
            shootAudioSource.Play();

        // 🔓 Desbloqueo según el tiempo que tú configures
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    private void ResetShoot()
    {
        isShooting = false;
    }
}






