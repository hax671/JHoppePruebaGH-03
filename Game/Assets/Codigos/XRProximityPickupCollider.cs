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

    [Header("Audio al recoger el objeto")]
    [SerializeField] private AudioSource pickupAudioSource;    // 🔊 NUEVO sonido de recogida

    private bool isPickingUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPickingUp) return;

        if (other.CompareTag("Item"))
        {
            StartCoroutine(PickupObject(other.gameObject));
        }
    }

    private System.Collections.IEnumerator PickupObject(GameObject item)
    {
        isPickingUp = true;

        // 🔊 Reproducir sonido de recoger
        if (pickupAudioSource != null)
            pickupAudioSource.Play();

        // Activar la animación
        animator.SetTrigger(triggerName);

        float elapsed = 0f;
        Vector3 startPos = item.transform.position;

        // Mover el objeto hacia la mano
        while (elapsed < destroyDelay)
        {
            if (item == null) yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (destroyDelay * 0.7f));
            item.transform.position = Vector3.Lerp(startPos, grabPoint.position, t);
            yield return null;
        }

        // Destruir el objeto
        if (item != null)
        {
            Destroy(item);
        }

        isPickingUp = false;
    }
}


