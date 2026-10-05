using UnityEngine;

public class CardVisual : MonoBehaviour
{
    private Renderer cardRenderer;

    void Awake()
    {
        cardRenderer = GetComponent<Renderer>();
    }

    public void CambiarTextura(Texture2D nuevaTextura)
    {
        if (cardRenderer != null && nuevaTextura != null)
        {
            cardRenderer.material.mainTexture = nuevaTextura;
        }
    }
}
