using UnityEngine;
using UnityEngine.InputSystem;

public class XRRunAndShootController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Run Settings")]
    [SerializeField] private string runParameter = "Run"; // Bool
    [SerializeField] private InputActionReference moveActionReference; // Vector2 (joystick o W/S)

    [Header("Shoot Settings")]
    [SerializeField] private string shootTrigger = "shoot"; // Trigger
    [SerializeField] private InputActionReference shootActionReference; // Button (gatillo o click)

    [Header("Audio Settings")]
    [SerializeField] private AudioSource shootAudioSource; // Arrastra aquí el AudioSource con tu sonido

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
        // Activar animación
        animator.SetTrigger(shootTrigger);

        // Reproducir sonido si existe el AudioSource
        if (shootAudioSource != null)
        {
            shootAudioSource.Play();
        }
    }
}



