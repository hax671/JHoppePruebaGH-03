using UnityEngine;

public class XRItemPickup : MonoBehaviour
{
    [Header("Animator del jugador")]
    [SerializeField] private Animator animator;

    [Header("Nombre del parámetro Trigger en el Animator")]
    [SerializeField] private string triggerName = "Take_item"; // o el nombre del trigger exacto

    [Header("Etiqueta de los objetos recogibles")]
    [SerializeField] private string itemTag = "Item"; // etiqueta del objeto a recoger

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador toca un objeto con esa etiqueta
        if (other.CompareTag(itemTag))
        {
            animator.SetTrigger(triggerName);

            // (Opcional) destruir o desactivar el objeto recogido
            // Destroy(other.gameObject);
        }
    }
}

