using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class XRKController : MonoBehaviour
{
    [Header("Enemy Check Settings")]
    public Transform enemyParent;          // Padre que contiene TODOS los enemigos
    public float checkInterval = 1f;       // Cada cuántos segundos revisa

    [Header("Skybox Settings")]
    public Material newSkybox;             // Skybox al derrotar enemigos

    [Header("Object Activation")]
    public GameObject objectToActivate;    // Objeto a activar al ganar

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad;  // Nombre de la escena a cargar
    [SerializeField] private float delayBeforeSceneChange = 3f; // Segundos antes del cambio

    private bool finished = false;

    private void Start()
    {
        StartCoroutine(CheckEnemiesRoutine());
    }

    private IEnumerator CheckEnemiesRoutine()
    {
        while (!finished)
        {
            if (enemyParent != null && enemyParent.childCount == 0)
            {
                finished = true;
                OnAllEnemiesDefeated();
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnAllEnemiesDefeated()
    {
        Debug.Log("✔ Todos los enemigos han sido derrotados.");

        // Cambiar skybox
        if (newSkybox != null)
            RenderSettings.skybox = newSkybox;

        // Activar objeto opcional
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Desactivar niebla
        RenderSettings.fog = false;

        // Iniciar el cambio de escena con espera
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        Debug.Log("⏳ Esperando " + delayBeforeSceneChange + " segundos antes de cambiar de escena...");

        yield return new WaitForSeconds(delayBeforeSceneChange);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("➡ Cambiando a la escena: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("⚠ No has asignado una escena en 'sceneToLoad'.");
        }
    }
}


