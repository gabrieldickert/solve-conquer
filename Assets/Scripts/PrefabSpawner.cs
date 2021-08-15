using UnityEngine;
public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int spawnSize = 1;
    public float spawnIntervalInSeconds = 10f;

    private float lastSpawnTime = 0f;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - lastSpawnTime > spawnIntervalInSeconds)
        {
            for(int i = 0; i < spawnSize; i++)
            {
                Instantiate(prefabToSpawn, gameObject.transform.position, gameObject.transform.rotation);
            }
            lastSpawnTime = Time.time;
        }
    }
}