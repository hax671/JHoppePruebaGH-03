using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{
    // GRAB
    public ActionBasedController ActionBasedController_grab;
    public XRRayInteractor xrRayInteractor_grab;
    public LineRenderer lineRenderer_grab;
    public XRInteractorLineVisual xrInteractorLineVisual_grab;

    // TELEPORT (solo rayo, teletransporte desactivado)
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

    private bool isTeleportMode = false;

    private TeleportationProvider teleportProvider; // <-- agregado

    private void Awake()
    {
        cc = xrOrigin.GetComponent<CharacterController>();
        teleportProvider = FindObjectOfType<TeleportationProvider>();

        // Importante: desactivar teletransporte desde el inicio
        if (teleportProvider != null)
            teleportProvider.enabled = false;
    }

    private void JoystickArribaPresionado(InputAction.CallbackContext context)
    {
        isTeleportMode = true;

        // Desactivar grab ray
        xrRayInteractor_grab.enabled = false;

        // Activar rayo de teleport (solo visual)
        xrRayInteractor_teleport.enabled = true;
        xrInteractorLineVisual_teleport.enabled = true;

        // Desactivar teletransporte REAL
        if (teleportProvider != null)
            teleportProvider.enabled = false;

        xrRayInteractor_teleport.enableUIInteraction = false;
    }

    private void JoystickArribaLiberado(InputAction.CallbackContext context)
        => Invoke(nameof(JoystickArribaLiberado_Invoke), 0.005f);

    private void JoystickArribaLiberado_Invoke()
    {
        if (!isTeleportMode)
            return;

        isTeleportMode = false;

        // Volver a activar grab ray
        xrRayInteractor_grab.enabled = true;

        // Apagar el ray del teleport
        xrRayInteractor_teleport.enabled = false;
        xrInteractorLineVisual_teleport.enabled = false;

        // ------------ BLOQUEAR DASH SI HAY PARED ----------------
        RaycastHit hit;
        Vector3 origin = xrOrigin.position + Vector3.up * 0.1f;
        Vector3 dir = xrOrigin.forward;

        if (Physics.Raycast(origin, dir, out hit, dashDistance))
        {
            Debug.Log("Dash bloqueado, pared detectada.");
            return; // <-- NO hacemos dash
        }

        // ---------------------------------------------------------

        // Dash direction
        dashDirection = xrOrigin.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        dashRemaining = dashDistance;
        isDashing = true;

        // Asegurar que teletransporte permanezca desactivado
        if (teleportProvider != null)
            teleportProvider.enabled = false;
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
        {
            isDashing = false;

            // Rehabilitar teletransporte (opcional)
            // teleportProvider.enabled = true; 
            // Lo dejo desactivado porque tú NO quieres teletransporte.
        }
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








