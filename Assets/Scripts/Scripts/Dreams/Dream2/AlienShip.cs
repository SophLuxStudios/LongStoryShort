using UnityEngine;

public class AlienShip : MonoBehaviour
{
    //used classes
    private EnemySpawner enemySpawner;
    private ObjectPooler objectPooler;

    //private fields
    private const float speed = 1.5f;
    private const float length = 4.5f;
    private const float fireRate = 1.2f;
    private Transform firePoint1;
    private Transform firePoint2;
    private Transform spaceCraft;
    private string returnTag;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        firePoint1 = transform.GetChild(0);
        firePoint2 = transform.GetChild(1);
        spaceCraft = GameObject.Find("SpaceCraft").transform;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        Movement();
        IsAlienShipPassed();
    }

    private void Movement()
    {
        transform.position = new Vector3((Mathf.PingPong(Time.time * speed, length) - length/2), transform.position.y, 0);
    }

    private void IsAlienShipPassed()
    {
        //Check if spaceCraft passed this ship
        if(spaceCraft.position.y - transform.position.y > 9)
        {
            //Enqueu if passed
            OnObjectReadyToEnqueue();
        }
    }

    public void GotShot()
    {
        AudioManager.Instance.Play("Explosion");
        OnObjectReadyToEnqueue();
    }

    private void Fire()
    {
        GameObject projectile1 = objectPooler.SpawnFromPool("AlienProjectile", firePoint1.position, firePoint1.rotation);
        projectile1.GetComponent<AlienProjectile>().OnObjectSpawn("AlienProjectile");

        GameObject projectile2 = objectPooler.SpawnFromPool("AlienProjectile", firePoint2.position, firePoint2.rotation);
        projectile2.GetComponent<AlienProjectile>().OnObjectSpawn("AlienProjectile");
    }

    public void OnObjectSpawn(string tag)
    {
        InvokeRepeating(nameof(Fire), 1f, fireRate);

        returnTag = tag;
    }

    public void OnObjectReadyToEnqueue()
    {
        CancelInvoke();
        objectPooler.ReturnPool(returnTag, gameObject);
        enemySpawner.SpawnEnemy();
    }
}