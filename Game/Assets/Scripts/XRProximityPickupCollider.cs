using UnityEngine;

public class XRProximityPickupCollider : MonoBehaviour
{
    [Header("Animator del jugador o brazo")]
    [SerializeField] private Animator animator;

    [Header("Nombre del parámetro Trigger en el Animator")]
    [SerializeField] private string triggerName = "Take";

    [Header("Tiempo antes de eliminar el objeto (segundos)")]
    [SerializeField] private float destroyDelay = 1.0f;

    [Header("Velocidad de movimiento del objeto hacia la mano")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Posición destino (por ejemplo, la mano del jugador)")]
    [SerializeField] private Transform grabPoint;

    private bool isPickingUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPickingUp) return; // evitar activar múltiples veces al mismo tiempo

        if (other.CompareTag("Item"))
        {
            StartCoroutine(PickupObject(other.gameObject));
        }
    }

    private System.Collections.IEnumerator PickupObject(GameObject item)
    {
        isPickingUp = true;

        // Activa la animación
        animator.SetTrigger(triggerName);

        float elapsed = 0f;
        Vector3 startPos = item.transform.position;

        // Mover el objeto suavemente hacia el punto de agarre
        while (elapsed < destroyDelay)
        {
            if (item == null) yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (destroyDelay * 0.7f)); // llega un poco antes de destruirse
            item.transform.position = Vector3.Lerp(startPos, grabPoint.position, t);
            yield return null;
        }

        // Destruir el objeto después del tiempo indicado
        if (item != null)
        {
            Destroy(item);
        }

        isPickingUp = false;
    }
}

