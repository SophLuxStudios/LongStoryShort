using UnityEngine;

public class AlienProjectile : MonoBehaviour
{
    //private fields
    private const float speed = 10f;
    private const float lifeSpan = 1f;
    private ObjectPooler objectPooler;
    private string projectileTag;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            OnObjectReadyToEnqueue();
        }
    }

    public void OnObjectSpawn(string projectileName)
    {
        CancelInvoke();
        Invoke(nameof(OnObjectReadyToEnqueue), lifeSpan);
        projectileTag = projectileName;
    }

    public void OnObjectReadyToEnqueue()
    {
        CancelInvoke();
        if(projectileTag == "AlienBossProjectile")
        {
            objectPooler.ReturnPool("AlienBossProjectile", gameObject);
        }
        else
        {
            objectPooler.ReturnPool("AlienProjectile", gameObject);
        }
    }
}