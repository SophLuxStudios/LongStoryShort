using UnityEngine;

public class Explosion : MonoBehaviour, IPooledObject
{
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnObjectSpawn()
    {
        animator.Play("ExplosionAnimation");

        Invoke(nameof(OnObjectReadyToEnqueue), 1.33f);
    }

    public void OnObjectReadyToEnqueue()
    {
        ObjectPooler.Instance.ReturnPool("Explosion", gameObject);
    }
}