using UnityEngine;
using System.Collections;

public class XRKController : MonoBehaviour
{
    [Header("Enemy Check Settings")]
    public Transform enemyParent;          // Padre que contiene TODOS los enemigos
    public float checkInterval = 1f;       // Cada cuántos segundos revisa

    [Header("Skybox Settings")]
    public Material newSkybox;             // Skybox que quieres poner al ganar

    [Header("Object Activation")]
    public GameObject objectToActivate;    // Prefab u objeto que quieres activar

    private bool finished = false;

    private void Start()
    {
        StartCoroutine(CheckEnemiesRoutine());
    }

    private IEnumerator CheckEnemiesRoutine()
    {
        while (!finished)
        {
            // Si el padre no tiene hijos, ya no hay enemigos
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

        // Activar el objeto o componente
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Desactivar la niebla
        RenderSettings.fog = false;
    }
}

