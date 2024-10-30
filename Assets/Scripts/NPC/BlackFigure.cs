using UnityEngine;

public class BlackFigure : MonoBehaviour
{
    //private fields
    private Animator animator;
    private const float speed = 3.5f;
    [SerializeField] private bool shouldWalk;
    private Vector3 targetPoint = new Vector3(1, 25.5f, 0);

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(shouldWalk)
        {
            Walk();
        }
    }

    public void StartWalking()
    {
        shouldWalk = true;
    }

    private void Walk()
    {
        if(Vector2.Distance(transform.position, targetPoint) > 0)
        {
            animator.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("isWalking", false);
            shouldWalk = false;
        }
    }
}