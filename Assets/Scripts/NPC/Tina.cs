using System.Collections;
using UnityEngine;

public class Tina : MonoBehaviour
{
    //used class'
    private DialogueManager dialogueManager;

    //private fields
    private Animator animator;
    private const float speed = 2.5f;
    private const float followingDistance = 1.3f;
    private Transform cansu;
    private Transform targetToFace;
    private Vector2 targetDirection;
    private Vector2 sittingPosition = new Vector2(2.35f, -0.8f);
    private Vector2 atRoomPosition = new Vector2(.4f, -.2f);
    [SerializeField] private Transform[] kitchenPathWaypoints;
    [SerializeField] private Transform[] doorPathWaypoints;
    [SerializeField] private Transform[] doorToKitchenPathWaypoints;
    [SerializeField] private Transform[] PooPathWaypoints;
    [SerializeField] private Transform[] CatchTinaWaypoints;
    [SerializeField] private GameObject PooPrefab;
    private int waypointIndex = 0;
    private bool returningKitchen;
    private bool tinaGoingOut;
    private bool tinaCameHome;
    [SerializeField] private bool tinaShitting;
    [SerializeField] private bool tinaRunningHome;
    private int shittingLocationIndex;

    //public variables
    public bool shouldFollowCansu;

    void Start()
    {
        animator = GetComponent<Animator>();
        cansu = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dialogueManager = FindObjectOfType<DialogueManager>();

        TinaSit(!GameManager.Instance.isTinaInHerRoom);

        if(GameManager.Instance.isTinaInHerRoom)
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }

        //Debug.Log("Tina came home is: " + tinaCameHome);
    }

    void FixedUpdate()
    {
        FaceTarget(targetToFace);
    
        if(shouldFollowCansu)
        {
            Follow(cansu, followingDistance);
            targetToFace = cansu;
        }
        else if(returningKitchen)
        {
            FollowPath(kitchenPathWaypoints);
        }
        else if(tinaGoingOut)
        {
            FollowPath(doorPathWaypoints);
        }
        else if(tinaCameHome)
        {
            FollowPath(doorToKitchenPathWaypoints);
        }
        else if(tinaShitting)
        {
            FollowPath(PooPathWaypoints);
        }
        else if(tinaRunningHome)
        {
            FollowPath(CatchTinaWaypoints);
            GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    private void FaceTarget(Transform target)
    {
        if(animator.GetBool("isWalking"))//Set direction while walking
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetFloat("X", GetTargetDirection(target).x);
            animator.SetFloat("Y", GetTargetDirection(target).y);
        }
        else//flip x while idle
        {
            if(GetTargetDirection(cansu).x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private Vector2 GetTargetDirection(Transform target)
    {
        return target.position - transform.position;
    }

    private void Follow(Transform target, float distance)
    {
        TinaSit(false);

        if(Vector2.Distance(transform.position, target.position) > distance)
        {
            if(!tinaRunningHome)
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            animator.SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            GetComponent<CircleCollider2D>().enabled = true;
            animator.SetBool("isWalking", false);
        }
    }

    private void FollowPath(Transform[] waypoints)
    {
        if(!tinaShitting && !tinaRunningHome)
        {
            GameManager.Instance.EnableInputs(false);
        }
        
        if(waypointIndex < waypoints.Length)
        {                    
            if(!Vector2.Equals((Vector2)transform.position, (Vector2)waypoints[waypointIndex].position))
            {
                targetToFace = waypoints[waypointIndex];
                Follow(waypoints[waypointIndex], 0f);
                //transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.fixedDeltaTime);
            }
            else
            {
                if(tinaShitting && shittingLocationIndex == waypointIndex)
                {
                    animator.SetBool("isShitting", true);
                    StartCoroutine(Wait(.15f));
                    Instantiate(PooPrefab, waypoints[waypointIndex].position, Quaternion.identity);
                    StartCoroutine(Wait(.15f));
                    animator.SetBool("isShitting", false);
                }
                waypointIndex++;
                //Debug.Log(waypointIndex);
            }
        }
        else
        {
            if(returningKitchen)
            {
                returningKitchen = false;
                TinaSit(true);
                GetComponent<CircleCollider2D>().enabled = true;
            }
            if(tinaGoingOut)
            {
                tinaGoingOut = false;
            }
            if(tinaCameHome)
            {
                TinaSit(true);
                tinaCameHome = false;
                //Debug.Log("Tina came home is: " + tinaCameHome);
                GetComponent<CircleCollider2D>().enabled = true;
            }

            if(tinaShitting)
            {
                if(!tinaRunningHome)
                {
                    shouldFollowCansu = true;
                }
                else
                {
                    TinaRanAway();
                }
                tinaShitting = false;
            }
            else if(tinaRunningHome)
            {
                tinaRunningHome = false;
                TinaSit(true);
            }
            waypointIndex = 0;
            animator.SetBool("isWalking", false);
            GameManager.Instance.EndAllEvents();
            GameManager.Instance.EnableInteractButton(true);
            if(dialogueManager.ReadyToMove())
            {
                GameManager.Instance.EnableMovement(true);
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void TinaSit(bool isSit)
    {
        animator.SetBool("isSitting", isSit);
    }

    public void GoToDoor()
    {
        GameManager.Instance.EnableInputs(false);
        tinaGoingOut = true;
    }

    public void ReturnKitchen()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        returningKitchen = true;
    }

    public void GoToKitchenFromDoor()
    {
        tinaCameHome = true;
    }

    public void GoToShit()
    {   
        shouldFollowCansu = false;
        tinaShitting = true;

        shittingLocationIndex = Random.Range(2, 6);
    }

    public void TinaRanAway()
    {
        shouldFollowCansu = false;
        tinaRunningHome = true;
    }

    public void TinaGotCaught()
    {
        shouldFollowCansu = true;
        tinaRunningHome = false;
    }
}