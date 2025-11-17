using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource audioSource;   // Reproductor de sonido
    [SerializeField] private AudioClip hoverSound;      // Sonido al pasar el cursor

    // Se ejecuta cuando el cursor entra sobre el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
            audioSource.PlayOneShot(hoverSound);
    }
}


