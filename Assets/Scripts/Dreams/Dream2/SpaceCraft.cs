using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceCraft : MonoBehaviour
{
    //used class'
    private SpaceShooterCamera spaceShooterCamera;
    private ObjectPooler objectPooler;

    //private fields
    private const float movementSpeed = 1.4f;
    private const float forwardSpeed = 1.6f;
    private Vector2 movementInput = Vector2.zero;
    private Rigidbody2D rb;
    private Transform flameLeft;
    private Transform flameRight;
    private Transform firePoint;
    private bool isFired;
    [SerializeField] private GameObject projectilePrefab;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        spaceShooterCamera = FindObjectOfType<SpaceShooterCamera>();
        rb = GetComponent<Rigidbody2D>();
        flameLeft = transform.GetChild(0);
        flameRight = transform.GetChild(1);
        firePoint = transform.GetChild(2);
    }

    void FixedUpdate()
    {
        Direction();
        Move();
    }

    private void Move()
    {
        Vector2 forwardMovement = new Vector2(0, forwardSpeed) * Time.fixedDeltaTime;

        if(transform.position.x > -3.5f && transform.position.x < 3.5f)
        {
            rb.MovePosition(rb.position + (movementInput * movementSpeed * Time.fixedDeltaTime) + forwardMovement);
        }
        else if(transform.position.x <= -3.5f)
        {
            if(movementInput.x < 0)
            {
                Vector2 direction = new Vector2(0, movementInput.y);
                rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime) + forwardMovement);
            }
            else
            {
                rb.MovePosition(rb.position + (movementInput * movementSpeed * Time.fixedDeltaTime) + forwardMovement);
            }
        }
        else if(transform.position.x >= 3.5f)
        {
            if(movementInput.x > 0)
            {
                Vector2 direction = new Vector2(0, movementInput.y);
                rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime) + forwardMovement);
            }
            else
            {
                rb.MovePosition(rb.position + (movementInput * movementSpeed * Time.fixedDeltaTime) + forwardMovement);
            }
        }
    }

    private void Direction()
    {
        //set rotation.z between -5 & 5 depending on input x
        float rotationAngle = movementInput.x * -5;
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

        //scale flames depending on input y
        float flameSpeedScaler = 0.7f + (movementInput.y / 5);
        //scale flames depending on input x
        float flameStirScaler = movementInput.x * 0.1f;

        flameLeft.localScale = new Vector3(0.45f, flameSpeedScaler - flameStirScaler, 1);
        flameRight.localScale = new Vector3(0.45f, flameSpeedScaler + flameStirScaler, 1);
    }

    private void Fire()
    {
        if(!isFired)
        {
            //Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            GameObject projectile = objectPooler.SpawnFromPool("Projectile", firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().OnObjectSpawn();
            isFired = true;

            StartCoroutine(FireRateCoroutine(0.5f));
        }
    }

    IEnumerator FireRateCoroutine(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);
        isFired = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyBullet")
        {
            other.GetComponent<AlienProjectile>().OnObjectReadyToEnqueue();
            StartCoroutine(CameraShakeCoroutine(0.2f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            GameObject explosion = objectPooler.SpawnFromPool("Explosion", collision.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().OnObjectSpawn();
            collision.collider.gameObject.GetComponent<AlienShip>().GotShot();
        }
        else if(collision.collider.tag == "EnemyBoss")
        {
            GameObject explosion = objectPooler.SpawnFromPool("Explosion", collision.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().OnObjectSpawn();
            collision.collider.gameObject.GetComponent<AlienBoss>().GotShot();
        }

        StartCoroutine(CameraShakeCoroutine(0.2f));
    }

    IEnumerator CameraShakeCoroutine(float shakeLength)
    {
        spaceShooterCamera.ActivateShake(true);
        yield return new WaitForSeconds(shakeLength);
        spaceShooterCamera.ActivateShake(false);
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnInteract()
    {
        Fire();
    }
}