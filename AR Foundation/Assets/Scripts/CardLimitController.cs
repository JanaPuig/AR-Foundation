using UnityEngine;

public class CardLimitController : MonoBehaviour
{
    [Header("Número máximo de cartas en la escena")]
    public int maxCards = 4; 
    public MonoBehaviour spawnTriggerToDisable;

    private int currentCardCount = 0;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    public void ActiveCard()
    {
        if (rayInteractor != null)
        {
            LayerMask mask = LayerMask.GetMask("Tablero");
            rayInteractor.raycastMask = mask;
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
