using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{
    //grab
    public ActionBasedController ActionBasedController_grab;
    public XRRayInteractor xrRayInteractor_grab;
    public LineRenderer lineRenderer_grab;
    public XRInteractorLineVisual xrInteractorLineVisual_grab;

    //teleport
    public ActionBasedController ActionBasedController_teleport;
    public XRRayInteractor xrRayInteractor_teleport;
    public LineRenderer lineRenderer_teleport;
    public XRInteractorLineVisual xrInteractorLineVisual_teleport;

    public InputActionReference Joystick_North_Ref;

    // DASH
    public float dashDistance = 3f;
    public float dashSpeed = 15f;
    public Transform xrOrigin;
    private CharacterController cc;

    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashRemaining;

    private bool isTeleportMode = false;  // <-- FIX

    private void Awake()
    {
        cc = xrOrigin.GetComponent<CharacterController>();
    }

    private void JoystickArribaPresionado(InputAction.CallbackContext context)
    {
        isTeleportMode = true; // <-- estamos en modo teletransporte

        xrRayInteractor_grab.enabled = false;

        xrRayInteractor_teleport.enabled = true;
        xrInteractorLineVisual_teleport.enabled = true;

        xrRayInteractor_teleport.enableUIInteraction = false;
    }

    private void JoystickArribaLiberado(InputAction.CallbackContext context)
        => Invoke(nameof(JoystickArribaLiberado_Invoke), 0.005f);

    private void JoystickArribaLiberado_Invoke()
    {
        // Si no estaba activo el rayo → NO hacemos dash
        if (!isTeleportMode)
            return;

        isTeleportMode = false; // salimos de teletransporte

        xrRayInteractor_grab.enabled = true;

        xrRayInteractor_teleport.enabled = false;
        xrInteractorLineVisual_teleport.enabled = false;

        // --- AHORA SÍ: solo hacemos dash si venía del modo teletransporte ---
        dashDirection = xrOrigin.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        dashRemaining = dashDistance;
        isDashing = true;
    }

    private void Update()
    {
        if (!isDashing) return;

        float step = dashSpeed * Time.deltaTime;

        if (step > dashRemaining)
            step = dashRemaining;

        cc.Move(dashDirection * step);

        dashRemaining -= step;

        if (dashRemaining <= 0f)
            isDashing = false;
    }

    private void OnEnable()
    {
        Joystick_North_Ref.action.performed += JoystickArribaPresionado;
        Joystick_North_Ref.action.canceled += JoystickArribaLiberado;
    }

    private void OnDisable()
    {
        Joystick_North_Ref.action.performed -= JoystickArribaPresionado;
        Joystick_North_Ref.action.canceled -= JoystickArribaLiberado;
    }
}


