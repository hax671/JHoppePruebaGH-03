using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Death Settings")]
    [SerializeField] private float deathDelay = 2f; // segundos para cambiar de escena
    [SerializeField] private string sceneToLoad = "GameOver"; // nombre de escena

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // ----------------------------------------
    //             RECIBIR DAÑO
    // ----------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        Debug.Log("Vida del jugador: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // ----------------------------------------
    //             RECUPERAR VIDA
    // ----------------------------------------
    public void Heal(float amount)
    {
        if (isDead) return; // No curar si ya está muerto

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Jugador curado: +" + amount + " | Vida actual: " + currentHealth);
    }

    // ----------------------------------------
    //             MUERTE
    // ----------------------------------------
    private void Die()
    {
        isDead = true;

        Debug.Log("Jugador muerto. Cambio de escena en " + deathDelay + " segundos.");

        Invoke(nameof(ChangeScene), deathDelay);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}


