using UnityEditor;
using UnityEngine;

public class PruebaRecibirTipoMonstruo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ARCardSpawner.OnMonsterTypeChanged += ActualizarTipo;
    }

    private void OnDisable()
    {
        ARCardSpawner.OnMonsterTypeChanged -= ActualizarTipo;
    }

    void ActualizarTipo(string TipoMonsJugador)
    {
        Debug.Log("Carta del jugador tipo: " + TipoMonsJugador);
    }
}
