using TMPro;
using UnityEngine;


// Lleva la partida: rondas, puntos y fin de partida.
//   - Victoria: +10 puntos y siguiente ronda.
//   - Empate:   siguiente ronda, sin puntos.
//   - Derrota:  fin de la partida, se muestran los puntos totales y se puede reiniciar.



public class RoundManager : MonoBehaviour
{
    [Header("Reglas")]
    [SerializeField] private int puntosPorVictoria = 10;

    [Header("UI durante la partida (opcional)")]
    [SerializeField] private TMP_Text textoPuntos;
    [SerializeField] private TMP_Text textoRonda;
    [SerializeField] private TMP_Text textoResultado;

    [Header("UI de fin de partida (opcional)")]
    [SerializeField] private GameObject panelFinPartida;
    [SerializeField] private TMP_Text textoPuntosFinales;

    public int Puntos { get; private set; }
    public int Ronda { get; private set; }
    public bool PartidaTerminada { get; private set; }

    private void Start()
    {
        EmpezarPartida();
    }

    //Volver a jugar
    public void EmpezarPartida()
    {
        Puntos = 0;
        Ronda = 1;
        PartidaTerminada = false;

        if (panelFinPartida != null) panelFinPartida.SetActive(false);
        if (textoResultado != null) textoResultado.text = "";
        ActualizarUI();
    }

    //Juega una ronda: el rival saca un tipo al azar.
    public RoundResult JugarRonda(ElementType tipoJugador)
    {
        return JugarRonda(tipoJugador, ElementSystem.RandomType());
    }

   
    public RoundResult JugarRonda(ElementType tipoJugador, ElementType tipoRival)
    {
        if (PartidaTerminada)
        {
            Debug.LogWarning("La partida ha terminado. Llama a EmpezarPartida() para volver a jugar.");
            return RoundResult.Derrota;
        }

        RoundResult resultado = ElementSystem.Resolve(tipoJugador, tipoRival);
        Debug.Log($"Ronda {Ronda}: {tipoJugador} vs {tipoRival} -> {resultado}");

        if (textoResultado != null)
            textoResultado.text = $"{tipoJugador} vs {tipoRival}: {resultado}";

        switch (resultado)
        {
            case RoundResult.Victoria:
                Puntos += puntosPorVictoria;
                Ronda++;
                break;

            case RoundResult.Empate:
                Ronda++;
                break;

            case RoundResult.Derrota:
                TerminarPartida();
                break;
        }

        ActualizarUI();
        return resultado;
    }

    private void TerminarPartida()
    {
        PartidaTerminada = true;
        Debug.Log($"Fin de la partida. Puntos totales: {Puntos}");

        if (textoPuntosFinales != null) textoPuntosFinales.text = $"Puntos: {Puntos}";
        if (panelFinPartida != null) panelFinPartida.SetActive(true);
    }

    private void ActualizarUI()
    {
        if (textoPuntos != null) textoPuntos.text = $"Puntos: {Puntos}";
        if (textoRonda != null) textoRonda.text = $"Ronda {Ronda}";
    }



    [ContextMenu("Probar: jugar ronda con tipo al azar")]
    private void ProbarRonda()
    {
        JugarRonda(ElementSystem.RandomType());
    }

    [ContextMenu("Probar: reiniciar partida")]
    private void ProbarReinicio()
    {
        EmpezarPartida();
    }
}
