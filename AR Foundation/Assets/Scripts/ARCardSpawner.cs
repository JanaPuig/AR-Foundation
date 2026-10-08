using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARCardSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct CardPrefabPair
    {
        public string imageName;
        public GameObject prefabToSpawn;
    }

    //Pasar tipo de monstruo al script que controle el combate
    public string MonsterType;
    public static event Action<string> OnMonsterTypeChanged;

    [Header("Asocia cada carta con su bicho")]
    public List<CardPrefabPair> cardPrefabs;

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> prefabDictionary;
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    private bool combateIniciadoParaEstaCarta = false;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // Convertimos la lista en un Diccionario para búsqueda rápida
        prefabDictionary = new Dictionary<string, GameObject>();
        foreach (var pair in cardPrefabs)
        {
            if (!string.IsNullOrEmpty(pair.imageName) && pair.prefabToSpawn != null)
            {
                if (!prefabDictionary.ContainsKey(pair.imageName))
                {
                    prefabDictionary.Add(pair.imageName, pair.prefabToSpawn);
                }
            }
        }
    }

    void OnEnable() => trackedImageManager.trackedImagesChanged += OnChanged;
    void OnDisable() => trackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateSpawnedObject(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateSpawnedObject(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            string imageName = trackedImage.referenceImage.name;
            if (spawnedObjects.ContainsKey(imageName))
            {
                Destroy(spawnedObjects[imageName]);
                spawnedObjects.Remove(imageName);
            }
        }
    }

    void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        if (trackedImage == null || trackedImage.referenceImage == null || string.IsNullOrEmpty(trackedImage.referenceImage.name))
        {
            return;
        }

        if (GameManager.Instance != null && !GameManager.Instance.tableroColocado)
        {
            if (GameManager.Instance.textoResultado != null)
                GameManager.Instance.textoResultado.text = "Place the board";
            return;
        }

        string imageName = trackedImage.referenceImage.name;
        bool isTracking = trackedImage.trackingState == TrackingState.Tracking;

        // Ahora es 100% seguro usar el diccionario porque imageName nunca serï¿½ null
        if (prefabDictionary.ContainsKey(imageName))
        {
            Debug.Log("Carta: " + imageName);
            SendCardType(imageName);
            if (!spawnedObjects.ContainsKey(imageName))
            {
                GameObject prefab = prefabDictionary[imageName];
                GameObject newPrefab = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                spawnedObjects.Add(imageName, newPrefab);

                if (GameManager.Instance != null && !GameManager.Instance.isRoundActive && !combateIniciadoParaEstaCarta)
                {
                    combateIniciadoParaEstaCarta = true;
                    GameManager.Instance.SetPlayerMonster(newPrefab);
                    MonsterType tipoDetectado = ObtenerTipoPorNombre(imageName);
                    GameManager.Instance.StartCombatSequence(tipoDetectado);
                }
            }
            else
            {
                GameObject spawnedObj = spawnedObjects[imageName];
                if (spawnedObj != null)
                {
                    spawnedObj.transform.position = trackedImage.transform.position;
                    spawnedObj.transform.rotation = trackedImage.transform.rotation;
                    spawnedObj.SetActive(isTracking);
                }
                else
                {
                    spawnedObjects.Remove(imageName);
                    combateIniciadoParaEstaCarta = false;
                }
            }
        }
    }

    void SendCardType(string cardtype)
    {
        MonsterType = cardtype;
        OnMonsterTypeChanged?.Invoke(cardtype);
    }

    private MonsterType ObtenerTipoPorNombre(string name)
    {
        if (name == "Agua") return global::MonsterType.Agua;
        if (name == "Electrico") return global::MonsterType.Electrico;
        if (name == "Volador") return global::MonsterType.Volador;
        if (name == "Planta") return global::MonsterType.Planta;
        return global::MonsterType.Fuego;
    }
}