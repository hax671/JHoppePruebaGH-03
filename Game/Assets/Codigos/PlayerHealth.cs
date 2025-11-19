using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Death Settings")]
    [SerializeField] private float deathDelay = 2f; // segundos para cambiar de escena
    [SerializeField] private string sceneToLoad = "GameOver";

    [Header("UI Settings")]
    [SerializeField] private Image healthFill; // ← asignar el Fill de la barra de vida

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // ----------------------------------------
    //             RECIBIR DAÑO
    // ----------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida del jugador: " + currentHealth);

        UpdateHealthUI();

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
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Jugador curado: +" + amount + " | Vida actual: " + currentHealth);

        UpdateHealthUI();
    }

    // ----------------------------------------
    //             ACTUALIZAR UI
    // ----------------------------------------
    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }

    // ----------------------------------------
    //                 MUERTE
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



