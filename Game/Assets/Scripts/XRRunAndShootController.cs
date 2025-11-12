using UnityEngine;
using UnityEngine.InputSystem;

public class XRRunAndShootController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Run Settings")]
    public string runParameter = "Run"; // Bool
    public InputActionReference moveActionReference; // Vector2 (joystick o W/S)

    [Header("Shoot Settings")]
    public string shootTrigger = "shoot"; // Trigger
    public InputActionReference shootActionReference; // Button (gatillo o click)

    private void OnEnable()
    {
        // Activar acciones
        moveActionReference.action.Enable();
        shootActionReference.action.Enable();

        // Suscribirse al disparo
        shootActionReference.action.performed += OnShoot;
    }

    private void OnDisable()
    {
        // Desactivar acciones
        moveActionReference.action.Disable();
        shootActionReference.action.Disable();

        // Desuscribirse del evento
        shootActionReference.action.performed -= OnShoot;
    }

    private void Update()
    {
        // --- CONTROL DE CORRER ---
        Vector2 moveValue = moveActionReference.action.ReadValue<Vector2>();

        // Si el joystick se mueve hacia arriba o W está presionada
        bool isRunning = moveValue.y > 0.5f;
        animator.SetBool(runParameter, isRunning);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        // --- CONTROL DE DISPARO ---
        animator.SetTrigger(shootTrigger);
    }
}

