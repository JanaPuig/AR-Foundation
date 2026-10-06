using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets; // Necesario para reconocer el ObjectSpawner

public class CardLimitController : MonoBehaviour
{
    [Header("Número máximo de cartas en la escena")]
    public int maxCards = 4;
    public MonoBehaviour spawnTriggerToDisable;

    private int currentCardCount = 0;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    [Header("Referencia al Spawner")]
    public ObjectSpawner objectSpawner;

    void OnEnable()
    {
        if (objectSpawner != null)
        {
            objectSpawner.objectSpawned += OnBoardSpawned;
        }
    }

    void OnDisable()
    {
        if (objectSpawner != null)
        {
            objectSpawner.objectSpawned -= OnBoardSpawned;
        }
    }
    private void OnBoardSpawned(GameObject spawnedBoard)
    {
        ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.enabled = false;
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }

        if (objectSpawner != null)
        {
            objectSpawner.enabled = false;
        }
    }

    public void OnCardSpawned()
    {
        currentCardCount++;

        if (currentCardCount >= maxCards)
        {
            if (spawnTriggerToDisable != null)
            {
                spawnTriggerToDisable.enabled = false;
            }
        }
    }
}