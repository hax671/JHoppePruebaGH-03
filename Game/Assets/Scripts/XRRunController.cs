using UnityEngine;
using UnityEngine.InputSystem;

public class XRRunController : MonoBehaviour
{
    public Animator animator;
    public string animationParameter = "Run";
    public InputActionReference moveActionReference; // acción Vector2

    private void OnEnable()
    {
        moveActionReference.action.Enable();
    }

    private void OnDisable()
    {
        moveActionReference.action.Disable();
    }

    private void Update()
    {
        // Leer el valor continuamente cada frame
        Vector2 moveValue = moveActionReference.action.ReadValue<Vector2>();

        // Si se mueve el joystick hacia arriba o presiona W (valor Y > 0.5)
        if (moveValue.y > 0.5f)
        {
            animator.SetBool(animationParameter, true);
        }
        else
        {
            animator.SetBool(animationParameter, false);
        }
    }
}






