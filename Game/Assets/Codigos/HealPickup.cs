using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log("Item recogido. Vida recuperada: " + healAmount);

            Destroy(gameObject); // desaparecer el item
        }
    }
}

