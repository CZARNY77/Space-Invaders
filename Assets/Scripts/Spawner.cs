using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [System.Serializable]
    public struct SpawnablePickups
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }
    [SerializeField] SpawnablePickups[] pickups;
    [SerializeField] GameObject asteroid;

    [SerializeField] float maxSpawnCount;
    [SerializeField] float minSpawnRate = 0.5f;
    [SerializeField] float maxSpawnRate = 1.5f;

    float edge;
    void Start()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
        edge = Camera.main.ScreenToWorldPoint(Vector2.zero).x + 1f;
    }

    public void Spawn()
    {
        if (!GameManager.instance.endGame && GameManager.instance.startGame)
        {
            int spawnCount = (int)Random.Range(0, maxSpawnCount);
            float spawnChance = Random.value;
            for (int i = 0; i < spawnCount; i++)
            {
                float Y = Random.Range(0, 5);
                float X = Random.Range(edge, -edge);
                GameObject prefab = asteroid;
                foreach(SpawnablePickups obj in pickups)
                {
                    if (spawnChance < obj.spawnChance)
                    {
                        prefab = obj.prefab;
                        spawnChance = Random.value;
                    }
                }

                GameObject obstacle = Instantiate(prefab);
                obstacle.transform.position = transform.position + new Vector3(X, Y, 0);

            }
            
        }
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
