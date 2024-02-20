using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private GameObject explosionPrefab;
    private const float speed = 9f;
    private const float lifeSpan = 2f;
    private ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            OnObjectReadyToEnqueue();
            GameObject explosion = objectPooler.SpawnFromPool("Explosion", collision.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().OnObjectSpawn();
            collision.collider.gameObject.GetComponent<AlienShip>().GotShot();
        }
        else if(collision.collider.tag == "EnemyBoss")
        {
            OnObjectReadyToEnqueue();
            GameObject explosion = objectPooler.SpawnFromPool("Explosion", collision.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().OnObjectSpawn();
            collision.collider.gameObject.GetComponent<AlienBoss>().GotShot();
        }
    }

    public void OnObjectSpawn()
    {
        CancelInvoke();
        Invoke("OnObjectReadyToEnqueue", lifeSpan);
    }

    public void OnObjectReadyToEnqueue()
    {
        objectPooler.ReturnPool("Projectile", gameObject);
    }
}