using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Lógica juego")]
    public int rondaActual = 1;
    public string nombreEscenaInicio = "PantallaInicio";

    [Header("Puntos del Coliseo")]
    public Transform puntoCartaPlayer;
    public Transform puntoCartaMaquina;

    [Header("Puntos de Spawn por Defecto")] // De moemnto no hace falta
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    [Header("UI del Juego")]
    public TextMeshProUGUI textoResultado;

    [Header("Monstruos")]
    public GameObject cactoroPrefab;
    public GameObject tribalPrefab; 
    public GameObject fishPrefab;
    public GameObject frogPrefab;
    public GameObject armabeePrefab;

    public GameObject cartaFisicaPrefab;


    [Header("Imagen Monstruos")]
    public Texture2D imagenCactoro;
    public Texture2D imagenTribal;
    public Texture2D imagenFish;
    public Texture2D imagenFrog;
    public Texture2D imagenArmabee;

    [HideInInspector] public bool isRoundActive = false;
    private GameObject currentPlayerMonster;
    private GameObject currentEnemyMonster;
    private GameObject currentEnemyCard;

    void Awake()
    {
        Instance = this;
        if (textoResultado != null) textoResultado.text = "Escanea una carta";
    }

    void Start()
    {
        GameObject coliseoEnEscena = GameObject.Find("Gladiator Low Poly Arena");
        if (coliseoEnEscena != null)
        {
            if (puntoCartaPlayer == null)
                puntoCartaPlayer = coliseoEnEscena.transform.Find("PuntoCartaPlayer");

            if (puntoCartaMaquina == null)
                puntoCartaMaquina = coliseoEnEscena.transform.Find("PuntoCartaMáquina");
        }

    }
    void Update()
    {
#if UNITY_EDITOR
        if (isRoundActive) return;

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame) SpawnPlayerKeyboard(MonsterType.Planta);
            else if (keyboard.digit2Key.wasPressedThisFrame) SpawnPlayerKeyboard(MonsterType.Fuego);
            else if (keyboard.digit3Key.wasPressedThisFrame) SpawnPlayerKeyboard(MonsterType.Agua);
            else if (keyboard.digit4Key.wasPressedThisFrame) SpawnPlayerKeyboard(MonsterType.Electrico);
            else if (keyboard.digit5Key.wasPressedThisFrame) SpawnPlayerKeyboard(MonsterType.Volador);
        }
#endif
    }

    private void SpawnPlayerKeyboard(MonsterType type)
    {
        ClearPreviousRound();
        currentPlayerMonster = SpawnMonster(type, playerSpawnPoint);
        StartCombatSequence(type);
    }

    public void StartCombatSequence(MonsterType playerType)
    {
        if (isRoundActive) return;

        isRoundActive = true;

        if (textoResultado != null) textoResultado.text = $"[Ronda {rondaActual}] ¡Rival Invocado!...";

        MonsterType enemyType = (MonsterType)Random.Range(0, 5);
        StartCoroutine(EnemyTurnAndResolve(playerType, enemyType, currentPlayerMonster));
    }

    public void SetPlayerMonster(GameObject monster)
    {
        if (currentPlayerMonster != null && currentPlayerMonster != monster && currentPlayerMonster.transform.parent == null)
        {
            Destroy(currentPlayerMonster);
        }
        currentPlayerMonster = monster;
    }

    private IEnumerator EnemyTurnAndResolve(MonsterType playerType, MonsterType enemyType, GameObject bichoJugador)
    {
        if (puntoCartaPlayer != null)
        {
            float distancia = Vector3.Distance(bichoJugador.transform.position, puntoCartaPlayer.position);
            float tolerancia = 0.12f; 

            while (distancia > tolerancia)
            {
                if (bichoJugador == null)
                {
                    isRoundActive = false;
                    if (textoResultado != null) textoResultado.text = "Escanea una carta";
                    yield break;
                }

                distancia = Vector3.Distance(bichoJugador.transform.position, puntoCartaPlayer.position);
                yield return null;
            }
        }

        if (textoResultado != null)
            textoResultado.text = $"[Ronda {rondaActual}] ¡El rival acepta el desafío!...";

        yield return new WaitForSeconds(1.0f);

        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        if (puntoCartaMaquina != null)
        {
            spawnPosition = puntoCartaMaquina.position;
            spawnRotation = puntoCartaMaquina.rotation;
        }
        else if (enemySpawnPoint != null)
        {
            spawnPosition = enemySpawnPoint.position;
            spawnRotation = enemySpawnPoint.rotation;
        }

        currentEnemyMonster = Instantiate(ObtenerPrefabPorTipo(enemyType), spawnPosition, spawnRotation);
        Debug.Log($"Enemigo maquina invocado en su zona: {enemyType}");

        if (currentEnemyMonster != null && bichoJugador != null)
        {
            currentEnemyMonster.transform.localScale = bichoJugador.transform.localScale;
        }

        if (cartaFisicaPrefab != null)
        {
            currentEnemyCard = Instantiate(cartaFisicaPrefab, spawnPosition, spawnRotation);

            CardVisual visualCarta = currentEnemyCard.GetComponentInChildren<CardVisual>();
            if (visualCarta != null)
            {
                Texture2D texturaElegida = ObtenerTexturaPorTipo(enemyType);
                visualCarta.CambiarTextura(texturaElegida);
            }
        }

        yield return new WaitForSeconds(1.5f);

        RoundResult result = CombatRules.Evaluate(playerType, enemyType);
        StartCoroutine(PlayCombatAnimations(result));
    }



    private IEnumerator PlayCombatAnimations(RoundResult result)
    {
        if (textoResultado != null)
        {
            if (result == RoundResult.Win)
            {
                textoResultado.text = "<color=green>¡VICTORIA!</color>";
                yield return new WaitForSeconds(3.0f);

                rondaActual++;
                textoResultado.text = $"<color=yellow>¡Avanzas a la Ronda {rondaActual}!</color>";
                yield return new WaitForSeconds(2.0f);

                ActualizarTextoRonda("Escanea tu siguiente carta...");
                ClearPreviousRound();
                isRoundActive = false;
            }
            else if (result == RoundResult.Lose)
            {
                int rondasSuperadas = rondaActual - 1;
                textoResultado.text = $"<color=red>¡GAME OVER!</color>\nHas superado {rondasSuperadas} rondas consecutivas.";
                yield return new WaitForSeconds(5.0f);

                textoResultado.text = "Volviendo a la pantalla de inicio...";
                yield return new WaitForSeconds(3.0f);
                VolverAlInicio();
            }
            else
            {
                textoResultado.text = "<color=yellow>¡EMPATE!</color>\nInténtalo de nuevo.";
                yield return new WaitForSeconds(3.0f);
                ActualizarTextoRonda("Vuelve a escanear tu carta...");
                ClearPreviousRound();
                isRoundActive = false;
            }

        }

        //ClearPreviousRound();
        //isRoundActive = false;
    }

    private void VolverAlInicio()
    {
        ClearPreviousRound();
        rondaActual = 1;
        isRoundActive = false;
        ActualizarTextoRonda("Escanea una carta");
    }

    private void ActualizarTextoRonda(string mensajeExtra)
    {
        if (textoResultado != null)
            textoResultado.text = $"<b>[RONDA {rondaActual}]</b>\n{mensajeExtra}";
    }
    private GameObject SpawnMonster(MonsterType type, Transform spawnPoint)
    {
        GameObject prefab = ObtenerPrefabPorTipo(type);
        if (prefab != null && spawnPoint != null)
        {
            return Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
        return null;
    }

    private GameObject ObtenerPrefabPorTipo(MonsterType type)
    {
        return type switch
        {
            MonsterType.Planta => cactoroPrefab,
            MonsterType.Fuego => tribalPrefab,
            MonsterType.Agua => fishPrefab,
            MonsterType.Electrico => frogPrefab,
            MonsterType.Volador => armabeePrefab,
            _ => null
        };
    }

    private Texture2D ObtenerTexturaPorTipo(MonsterType type)
    {
        return type switch
        {
            MonsterType.Planta => imagenCactoro,
            MonsterType.Fuego => imagenTribal,
            MonsterType.Agua => imagenFish,
            MonsterType.Electrico => imagenFrog,
            MonsterType.Volador => imagenArmabee,
            _ => null
        };
    }

    private void ClearPreviousRound()
    {
        if (currentEnemyCard != null) Destroy(currentEnemyCard);
        if (currentEnemyMonster != null) Destroy(currentEnemyMonster);
        currentEnemyMonster = null;
        currentEnemyCard = null;

        if (currentPlayerMonster != null && currentPlayerMonster.transform.parent == null)
        {
            Destroy(currentPlayerMonster);
        }
    }
}
