using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    [HideInInspector] public bool isRoundActive = false;
    private GameObject currentPlayerMonster;
    private GameObject currentEnemyMonster;

    void Awake()
    {
        Instance = this;
        if (textoResultado != null) textoResultado.text = "Escanea una carta o pulsa 1-5";
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
        isRoundActive = true;

        if (textoResultado != null) textoResultado.text = "¡Rival Invocado!...";

        MonsterType enemyType = (MonsterType)Random.Range(0, 5);
        StartCoroutine(EnemyTurnAndResolve(playerType, enemyType));
    }

    public void SetPlayerMonster(GameObject monster)
    {
        if (currentPlayerMonster != null && currentPlayerMonster != monster && currentPlayerMonster.transform.parent == null)
        {
            Destroy(currentPlayerMonster);
        }
        currentPlayerMonster = monster;
    }

    private IEnumerator EnemyTurnAndResolve(MonsterType playerType, MonsterType enemyType)
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        //  enemigo a 1.5 metros del jugador
        if (currentPlayerMonster != null)
        {
            float distanciaRival = 1.5f;
            spawnPosition = currentPlayerMonster.transform.position + (currentPlayerMonster.transform.forward * distanciaRival);
            spawnPosition.y = currentPlayerMonster.transform.position.y;

            Vector3 direccionHaciaPlayer = currentPlayerMonster.transform.position - spawnPosition;
            direccionHaciaPlayer.y = 0;
            if (direccionHaciaPlayer != Vector3.zero)
            {
                spawnRotation = Quaternion.LookRotation(direccionHaciaPlayer);
            }
        }
        else if (enemySpawnPoint != null)
        {
            spawnPosition = enemySpawnPoint.position;
            spawnRotation = enemySpawnPoint.rotation;
        }

        // Enemigo máquina
        currentEnemyMonster = Instantiate(ObtenerPrefabPorTipo(enemyType), spawnPosition, spawnRotation);
        Debug.Log($"Enemigo maquina: {enemyType}");

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
            }
            else if (result == RoundResult.Lose)
            {
                textoResultado.text = "<color=red>¡DERROTA!</color>";
                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                textoResultado.text = "<color=yellow>¡EMPATE!</color>";
                yield return new WaitForSeconds(3.0f);
            }

            textoResultado.text = "Pon otra carta o pulsa tecla...";
        }

        ClearPreviousRound();
        isRoundActive = false;
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

    private void ClearPreviousRound()
    {
        if (currentEnemyMonster != null) Destroy(currentEnemyMonster);
        if (currentPlayerMonster != null && currentPlayerMonster.transform.parent == null)
        {
            Destroy(currentPlayerMonster);
        }
    }
}
