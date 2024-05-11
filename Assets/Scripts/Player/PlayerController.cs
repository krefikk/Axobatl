using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Singleton
    public static PlayerController player;
    
    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Combat")]
    float timeBetweenAutomaticGunShots = 0.2f;
    float timeBetweenRevolverShots = 0.5f;
    float timeBetweenShotgunShots = 0.6f;
    float timeSinceLastShoot;

    [Header("Gun")]
    public int gun; // 0 represents automatic gun, 1 represents revolver, 2 represents shotgun

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
    bool dashing = false;

    [Header("Skills")]
    bool canDash = true;

    [Header("Dash")]
    float dashSpeed = 15f;
    float dashTime = 0.2f;
    float dashCooldown = 1f;

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
        if (dashing) { return; } // Movement functions won't get called if player is dashing
        Move();
        StartDash();
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
        if (gun == 0) 
        { // If gun is automatic gun
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (attacking && timeSinceLastShoot >= timeBetweenAutomaticGunShots)
            {
                ShootAutoGunBullet();
                timeSinceLastShoot = 0;
            }
        }
        else if (gun == 1)
        { // If gun is revolver
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (attacking && timeSinceLastShoot >= timeBetweenRevolverShots)
            {
                ShootRevolverBullet();
                timeSinceLastShoot = 0;
            }
        }
        else if (gun == 2)
        { // If gun is shotgun
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (attacking && timeSinceLastShoot >= timeBetweenShotgunShots)
            {
                ShootShotgunBullets();
                timeSinceLastShoot = 0;
            }
        }
    }

    void ShootAutoGunBullet() // Shoots bullet
    {
        // Creates the bullet at 2 unit ahead of player
        Vector3 bulletSpawnPos = transform.position + GetDirection() * 0.5f;
        // Creates a bullet object with the exact same direction and rotation values as player
        GameObject bulletP = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        BulletHandler bullet = bulletP.GetComponent<BulletHandler>();
    }

    void ShootRevolverBullet() // Shoots bullet
    {
        // Creates the bullet at 2 unit ahead of player
        Vector3 bulletSpawnPos = transform.position + GetDirection() * 0.5f;
        // Creates a bullet object with the exact same direction and rotation values as player
        GameObject bulletP = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        BulletHandler bullet = bulletP.GetComponent<BulletHandler>();
        bullet.setDamage(5f);
    }

    void ShootShotgunBullets() // Shoots bullets
    {
        // Creates the bullet at 2 unit ahead of player
        Vector3 bulletSpawnPos = transform.position + GetDirection() * 0.5f;
        // Creates a bullet object with the exact same direction and rotation values as player
        GameObject bulletP1 = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        GameObject bulletP2 = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        GameObject bulletP3 = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        BulletHandler bullet1 = bulletP1.GetComponent<BulletHandler>();
        BulletHandler bullet3 = bulletP3.GetComponent<BulletHandler>();
        float spreadAngle1 = Random.Range(-10f, 10f);
        float spreadAngle3 = Random.Range(-10f, 10f);
        Vector3 spreadDirection1 = Quaternion.AngleAxis(spreadAngle1, Vector3.forward) * GetDirection();
        Vector3 spreadDirection3 = Quaternion.AngleAxis(spreadAngle3, Vector3.forward) * GetDirection();
        spreadDirection1.z = 0;
        spreadDirection1.Normalize();
        spreadDirection3.z = 0;
        spreadDirection3.Normalize();
        bullet1.setDirection(spreadDirection1);
        bullet3.setDirection(spreadDirection3);
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

    // --------------------------------Player Skills------------------------------------------
    void StartDash() 
    {
        if (Input.GetButtonDown("Dash") && canDash) 
        {
            Debug.Log("Dashed");
            StartCoroutine(Dash());
        }
    }
    
    IEnumerator Dash()
    {
        canDash = false;
        dashing = true;
        // Trigger animation
        Vector2 direction = new Vector2(xAxis, yAxis);
        if (direction.magnitude == 0)
        {
            rb.velocity = GetDirection() * dashSpeed;
            Debug.Log("No movement");
        }
        else
        {
            direction.Normalize();
            rb.velocity = direction * dashSpeed;
        }
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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

    public bool isDead() 
    {
        return dead;
    }
}

