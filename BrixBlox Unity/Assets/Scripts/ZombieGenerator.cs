using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 5f;
    
    void Start()
    {
        InvokeRepeating("SpawnZombie", 0f, spawnInterval);
    }

    void SpawnZombie()
    {
        Instantiate(zombiePrefab, transform.position, transform.rotation);
    }
}
