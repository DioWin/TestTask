using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnerController : MonoBehaviour
{
    [SerializeField] private List<FruitController> fruitPrefabs;
    [SerializeField] private Transform spawmHolder;
    [SerializeField] private Transform endCorner;

    [SerializeField] private Vector2 spawnRateRage;
    private float lastSpawnTime;

    private float currentSpawnRate;
    private float currentTime;

    private float fruitSpeed;

    private bool isEnable;

    private void Awake()
    {
        currentTime = Time.time;
        currentSpawnRate = Random.Range(spawnRateRage.x, spawnRateRage.y);
    }

    private void Update()
    {
        if (!isEnable)
            return;

        currentTime = Time.time;

        if (currentTime >= lastSpawnTime + currentSpawnRate)
        {
            lastSpawnTime = currentTime;
            SpawnFruit();
        }
    }

    private void SpawnFruit()
    {
        FruitController currentFruit = GetRandomFruit();

        Quaternion randomRotation = Random.rotation;

        Instantiate(currentFruit, spawmHolder).Initalize(fruitSpeed, randomRotation, endCorner);
    }

    private FruitController GetRandomFruit()
    {
        int randomIndex = Random.Range(0, fruitPrefabs.Count);

        return fruitPrefabs[randomIndex];
    }

    public void ChangeSpawnerStatus(bool isEnable)
    {
        this.isEnable = isEnable;
    }

    public void SetSpeed(float speed)
    {
        this.fruitSpeed = speed;
    }

    public void Eliminate()
    {
        ChangeSpawnerStatus(false);
    }
}
