using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UITiltEffectXR : MonoBehaviour
{
    [Header("Sensibilidad del efecto")]
    [SerializeField] private float rotationAmount = 10f;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Referencia del rayo derecho (VR)")]
    [SerializeField] private XRRayInteractor rightRay;   // Solo el Ray Interactor derecho

    private RectTransform rectTransform;
    private Vector3 initialRotation;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRotation = rectTransform.localEulerAngles;
    }

    private void Update()
    {
        Vector3? targetPoint = null;

        // 🔹 1️⃣ — Detectar punto de impacto con el rayo derecho
        if (rightRay != null && rightRay.TryGetCurrent3DRaycastHit(out RaycastHit rightHit))
        {
            targetPoint = rightHit.point;
        }

        // 🔹 2️⃣ — Si no hay rayos (modo PC), usar la posición del mouse
        if (targetPoint == null)
        {
            Vector3 mousePos = Input.mousePosition;
            float x = (mousePos.x / Screen.width - 0.5f) * 2f;
            float y = (mousePos.y / Screen.height - 0.5f) * 2f;
            ApplyTilt(x, y);
        }
        else
        {
            // Convertir el punto del Raycast al espacio de pantalla
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(targetPoint.Value);
            float x = (screenPoint.x / Screen.width - 0.5f) * 2f;
            float y = (screenPoint.y / Screen.height - 0.5f) * 2f;
            ApplyTilt(x, y);
        }
    }

    private void ApplyTilt(float x, float y)
    {
        // Calcular rotación deseada según movimiento
        Quaternion targetRotation = Quaternion.Euler(
            initialRotation.x - y * rotationAmount,
            initialRotation.y + x * rotationAmount,
            initialRotation.z
        );

        // Aplicar suavemente la rotación
        rectTransform.localRotation = Quaternion.Lerp(
            rectTransform.localRotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }
}



