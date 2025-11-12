using UnityEngine;
using UnityEngine.InputSystem;

public class XRShotController : MonoBehaviour
{
    public Animator animator;               // Asigna tu Animator principal
    public InputActionReference shootAction; // Asigna aquí tu acción "Shoot"

    private void OnEnable()
    {
        shootAction.action.performed += OnShoot;
        shootAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.performed -= OnShoot;
        shootAction.action.Disable();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        animator.SetTrigger("shoot");
    }
}

