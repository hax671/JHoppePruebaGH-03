using UnityEngine;
using UnityEngine.InputSystem;

public class XRKickController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Kick Animation")]
    [SerializeField] private string kickTrigger = "Kick"; // Trigger de la animación

    [Header("Input")]
    [SerializeField] private InputActionReference kickActionReference;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // ÚNICO AudioSource
    [SerializeField] private AudioClip kickSound;     // Sonido del golpe

    private void OnEnable()
    {
        kickActionReference.action.Enable();
        kickActionReference.action.performed += OnKick;
    }

    private void OnDisable()
    {
        kickActionReference.action.performed -= OnKick;
        kickActionReference.action.Disable();
    }

    private void OnKick(InputAction.CallbackContext context)
    {
        animator.SetTrigger(kickTrigger);
    }

    // -----------------------------------------------------
    // LLAMADO DESDE UN ANIMATION EVENT EN EL GOLPE
    // -----------------------------------------------------
    public void PlayKickSound()
    {
        if (kickSound != null && audioSource != null)
            audioSource.PlayOneShot(kickSound);
    }
}


