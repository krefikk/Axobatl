using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Singleton
    public static PlayerController player;
    
    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Combat")]
    float timeBetweenShoots = 0.15f;
    float timeSinceLastShoot;

    [Header("Health")]
    float maxHealth = 20;
    float health = 20;

    [Header("Movement")]
    private float xAxis = 0;
    private float yAxis = 0;
    public float walkSpeed = 5f;

    [Header("Flags")]
    bool attacking = false;
    bool dead = false;
    bool moving = false;

    // Components
    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        if (player != null && player != this)
        {
            Destroy(gameObject);
        }
        else
        {
            player = this;
        }
    }

    private void Start()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInputs();
        FaceMouse();
        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetInputs() 
    {
        xAxis = Input.GetAxis("Horizontal"); // Checks if player pressed left, right or A, D
        yAxis = Input.GetAxis("Vertical"); // Checks if player pressed up, down or W, S
        attacking = Input.GetMouseButtonDown(0); // Checks if player pressed left mouse button
    }

    void Move()
    {
        Vector2 movement = new Vector2(xAxis, yAxis) * walkSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
        // anim.SetBool("Walking", movement.magnitude > 0);

        if (movement.magnitude > 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    void Shoot() // Starts shooting
    {
        timeSinceLastShoot += Time.deltaTime;
        // Checks if player pressed attack button, and enough time passed since last shoot
        if (attacking && timeSinceLastShoot >= timeBetweenShoots) 
        {
            ShootBullet();
            timeSinceLastShoot = 0;
        }
    }

    void ShootBullet() // Shoots bullet
    {
        // Creates the bullet at 2 unit ahead of player
        Vector3 bulletSpawnPos = transform.position + GetDirection() * 0.5f;
        // Creates a bullet object with the exact same direction and rotation values as player
        GameObject bulletP = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        BulletHandler bullet = bulletP.GetComponent<BulletHandler>();
    }

    void Die() 
    {
        dead = true;
    }

    public void TakeDamage(float damage) 
    {
        // If damage taken is greater than enemy's current health points, kill the enemy
        if (damage >= health)
        {
            Debug.Log("Died");
            // Play some animations or sounds here
        }
        // If damage is not enough to kill the enemy, just decrease their health
        else
        {
            health -= damage;
            Debug.Log("Took damage");
            // Play some animations or sounds here
        }
    }

    void FaceMouse() // Faces the player to the direction where cursor stands
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0f; // Ensure the direction is in the XY plane
        direction.Normalize(); // Normalize the direction vector to have a magnitude of 1

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the character to face the mouse direction
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // --------------------------------Getters and Setters------------------------------------------
    public Vector3 GetDirection() // Returns the player's current look direction as a vector
    {
        float radians = GetRotationAngle() * Mathf.Deg2Rad;
        Vector3 facingDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        facingDirection.Normalize();
        return facingDirection;
    }

    public float GetRotationAngle() // Returns the player's current rotation value as "degrees"
    {
        return transform.rotation.eulerAngles.z;
    }
}
