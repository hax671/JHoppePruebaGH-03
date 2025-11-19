using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Game";   // La escena que se carga al presionar Play
    [SerializeField] private string autoSceneName = "Game";   // La escena a cargar automáticamente
    [SerializeField] private float autoLoadDelay = 0f;        // Tiempo antes de cargar automáticamente (0 = desactivado)

    private void Start()
    {
        // Si autoLoadDelay es mayor a 0, empezamos la cuenta regresiva
        if (autoLoadDelay > 0f)
        {
            StartCoroutine(AutoLoadSceneRoutine());
        }
    }

    // ---------------------------------------
    //          BOTÓN PLAY
    // ---------------------------------------
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // ---------------------------------------
    //       CAMBIAR A OTRA ESCENA MANUAL
    // ---------------------------------------
    public void LoadNextScene()
    {
        SceneManager.LoadScene(autoSceneName);
    }

    // ---------------------------------------
    //     CAMBIAR DE ESCENA AUTOMÁTICAMENTE
    // ---------------------------------------
    private IEnumerator AutoLoadSceneRoutine()
    {
        yield return new WaitForSeconds(autoLoadDelay);
        SceneManager.LoadScene(autoSceneName);
    }

    // ---------------------------------------
    //               SALIR DEL JUEGO
    // ---------------------------------------
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

