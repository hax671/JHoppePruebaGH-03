using UnityEngine;
using UnityEngine.InputSystem;

public class XRKickController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Kick Settings")]
    [SerializeField] private string kickTrigger = "Kick"; // nombre del Trigger en el Animator
    [SerializeField] private InputActionReference kickActionReference; // acción del botón o gatillo

    private void OnEnable()
    {
        // Activamos la acción del Input System y nos suscribimos al evento
        kickActionReference.action.Enable();
        kickActionReference.action.performed += OnKick;
    }

    private void OnDisable()
    {
        // Desactivamos y eliminamos la suscripción
        kickActionReference.action.performed -= OnKick;
        kickActionReference.action.Disable();
    }

    private void OnKick(InputAction.CallbackContext context)
    {
        // Dispara la animación (una sola vez)
        animator.SetTrigger(kickTrigger);
    }
}

