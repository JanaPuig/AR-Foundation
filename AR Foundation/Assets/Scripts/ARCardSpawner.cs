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
        string imageName = trackedImage.referenceImage.name;
        bool isTracking = trackedImage.trackingState == TrackingState.Tracking;

        if (prefabDictionary.ContainsKey(imageName))
        {
            Debug.Log("Carta: " + imageName);
            SendCardType(imageName);
            if (!spawnedObjects.ContainsKey(imageName))
            {
                GameObject prefab = prefabDictionary[imageName];
                GameObject newPrefab = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                spawnedObjects.Add(imageName, newPrefab);
                Debug.Log("Carta2: " + imageName);
            }
            else
            {
                GameObject spawnedObj = spawnedObjects[imageName];
                spawnedObj.transform.position = trackedImage.transform.position;
                spawnedObj.transform.rotation = trackedImage.transform.rotation;
                spawnedObj.SetActive(isTracking);
                Debug.Log("Carta3: " + imageName);
            }
        }
    }

    void SendCardType(string cardtype)
    {
        MonsterType = cardtype;
        OnMonsterTypeChanged?.Invoke(cardtype);
    }
}