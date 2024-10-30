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
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            OnObjectReadyToEnqueue();
            GameObject explosion = objectPooler.SpawnFromPool("Explosion", collision.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().OnObjectSpawn();
            collision.collider.gameObject.GetComponent<AlienShip>().GotShot();
        }
        else if(collision.collider.CompareTag("EnemyBoss"))
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
        Invoke(nameof(OnObjectReadyToEnqueue), lifeSpan);
    }

    public void OnObjectReadyToEnqueue()
    {
        objectPooler.ReturnPool("Projectile", gameObject);
    }
}