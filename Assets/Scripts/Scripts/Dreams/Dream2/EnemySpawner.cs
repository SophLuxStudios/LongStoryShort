using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //private fields
    [SerializeField] private Transform spaceCraft;
    [SerializeField] private GameObject bossHealthBar;
    private ObjectPooler objectPooler;
    private string[] aliensToSpawn = new string[]{"AlienShip1", "AlienShip2", "AlienShip1", "AlienShip2", "AlienBoss"};
    private int spawnCount = 0;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        Invoke(nameof(SpawnEnemy), .5f);
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(0, spaceCraft.position.y + 15, 0);

        GameObject enemy = objectPooler.SpawnFromPool(aliensToSpawn[spawnCount], spawnPosition, Quaternion.Euler(0, 0, -180));
        if(spawnCount < 4)
        {
            enemy.GetComponent<AlienShip>().OnObjectSpawn(aliensToSpawn[spawnCount]);
        }
        else
        {
            bossHealthBar.SetActive(true);
        }

        spawnCount++;
    }
}