using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    //used classes
    private ObjectPooler objectPooler;
    private BossHealth bossHealth;

    //private fields
    private const float speed = 2f;
    private const float length = 4f;
    private const float fireRate = 1.2f;
    private int health = 5;
    private Transform[] firePoints = new Transform[6];
    private Transform spaceCraft;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        bossHealth = FindObjectOfType<BossHealth>();
        spaceCraft = GameObject.Find("SpaceCraft").transform;

        for(int i = 0; i < 6; i++)
        {
            firePoints[i] = transform.GetChild(i);
        }

        InvokeRepeating(nameof(Fire), 1f, 1f);
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * speed, length) - length/2, spaceCraft.position.y + 10, 0);
    }

    private void Fire()
    {
        for(int i = 0; i < 6; i++)
        {
            string shipTag;

            if(i < 2)
            {
                shipTag = "AlienBossProjectile";
            }
            else
            {
                shipTag = "AlienProjectile";
            }

            GameObject projectile = objectPooler.SpawnFromPool(shipTag, firePoints[i].position, firePoints[i].rotation);
            projectile.GetComponent<AlienProjectile>().OnObjectSpawn(shipTag);
        }
    }

    public void GotShot()
    {
        AudioManager.Instance.Play("Explosion");
        health--;
        bossHealth.DecreaseBossHealth();

        if(health == 0)
        {
            Destroy(gameObject);

            GameManager.Instance.LoadIndoorScene("Dream");
        }
    }

    public void OnObjectSpawn()
    {
        InvokeRepeating(nameof(Fire), .5f, fireRate);
    }
}