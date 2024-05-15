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
    protected int gun = 0; // 0 represents automatic gun, 1 represents revolver, 2 represents shotgun

    [Header("Health")]
    private float maxHealth = 50;
    private float health = 50;

    [Header("Movement")]
    private float xAxis = 0;
    private float yAxis = 0;
    public float walkSpeed = 5f;

    [Header("Flags")]
    bool attacking = false;
    bool dead = false;
    bool moving = false;
    bool dashing = false;
    public bool inCardsScene = false;
    // Attributes
    bool mirrorArmorActivated = false;

    [Header("Skills")]
    bool canDash = true;

    [Header("Dash")]
    float dashSpeed = 15f;
    float dashTime = 0.2f;
    float dashCooldown = 1f;

    [Header("Perks")]
    float bulletSpeedMultiplier = 1;
    float bulletDamageMultiplier = 1;
    bool unlockedKatanaDash = false;

    [Header("Attributes")]
    int attribute; // Represents which attribute player choosed: 0 represents mirror armor
    float attributeUsingTime = 0;

    [Header("Mirror Armor")]
    float mirrorArmorCooldown = 20f;
    float mirrorArmorTime = 5f;
    bool isMirrorArmorOnCooldown = false;

    [Header("HUD")]
    public StatBar healthBar;

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
        DontDestroyOnLoad(this.gameObject);
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
        DisplayHealth();
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
        if (Input.GetKeyDown(KeyCode.Escape)) // Stops the game if game is flowing, starts the game if game is stopped
        {
            GameManager.gameManager.OpenCardsScene();
            inCardsScene = true;
            this.gameObject.SetActive(false);
        }
        //------------------------------Attribute Inputs-------------------------------------
        if (Input.GetKeyDown(KeyCode.Z) && !mirrorArmorActivated && !isMirrorArmorOnCooldown && attribute == 0)
        {
            ActivateMirrorArmor();
        }
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
        Vector3 bulletSpawnPosition = transform.position + GetDirection();
        Vector3 bulletDirection = GetDirection();

        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(bulletDirection.normalized);
            bullet.SetSpeed(10f * bulletSpeedMultiplier);
            bullet.SetDamage(2f * bulletDamageMultiplier);
            bullet.SetParent(gameObject);
        }
    }

    void ShootRevolverBullet() // Shoots bullet
    {
        Vector3 bulletSpawnPosition = transform.position + GetDirection();
        Vector3 bulletDirection = GetDirection();

        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(bulletDirection.normalized);
            bullet.SetSpeed(10f * bulletSpeedMultiplier);
            bullet.SetDamage(5f * bulletDamageMultiplier);
            bullet.SetParent(gameObject);
        }
    }

    void ShootShotgunBullets() // Shoots bullets
    {
        float spreadAngle1 = Random.Range(-10f, 10f);
        float spreadAngle3 = Random.Range(-10f, 10f);
        Vector3 spreadDirection1 = Quaternion.AngleAxis(spreadAngle1, Vector3.forward) * GetDirection();
        Vector3 spreadDirection3 = Quaternion.AngleAxis(spreadAngle3, Vector3.forward) * GetDirection();
        spreadDirection1.z = 0;
        spreadDirection1.Normalize();
        spreadDirection3.z = 0;
        spreadDirection3.Normalize();

        Vector3 bulletSpawnPosition = transform.position + GetDirection();
        Vector3 bulletDirection = GetDirection();

        for (int i = 0; i < 3; i++) 
        {
            GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            if (bullet != null)
            {
                if (i == 0)
                {
                    bullet.SetDirection(spreadDirection1);
                }
                else if (i == 2)
                {
                    bullet.SetDirection(spreadDirection3);
                }
                else 
                {
                    bullet.SetDirection(bulletDirection.normalized);
                }
                bullet.SetSpeed(10f * bulletSpeedMultiplier);
                bullet.SetDamage(2f * bulletDamageMultiplier);
                bullet.SetParent(gameObject);
            }
        }
    }

    public void DisplayHealth() 
    {
        healthBar.current = 100 * health / maxHealth;
    }

    public void Die() 
    {
        dead = true;
    }

    public void TakeDamage(float damage) 
    {
        if (!dashing) // Can't take damage while dashing
        {
            // If damage taken is greater than enemy's current health points, kill the enemy
            if (damage >= health)
            {
                Die();
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

    // --------------------------------Attributes------------------------------------------
    IEnumerator MirrorArmor() 
    {
        // Set skill active
        mirrorArmorActivated = true;
        // Play animation
        // Wait for skill duration
        yield return new WaitForSeconds(mirrorArmorTime);
        // Stop animation
        // Set skill inactive
        mirrorArmorActivated = false;
        // Start cooldown
        isMirrorArmorOnCooldown = true;
    }

    public void ActivateMirrorArmor()
    {
        StartCoroutine(MirrorArmor());
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

    public int GetGun() 
    {
        return gun;
    }

    public bool getKatanaDashStatus() 
    {
        return unlockedKatanaDash;
    }

    public bool isDashing() 
    {
        return dashing;
    }

    public bool isMirrorArmorOn() 
    {
        return mirrorArmorActivated;
    }

    //--------------------------------Perks------------------------------------------
    // ------Health
    public void HealthBoost()
    { // Increases max health and refreshes the health     
        maxHealth += 15;
        health = maxHealth;   
    }
    public void AppleJuice()
    { // Increases max health, refreshes health but slightly decreases walk speed.
        maxHealth += 30;
        health = maxHealth;
        walkSpeed -= 1;
    }

    public void Fortress()
    { // Greatly increases maximum health, replenishes health but also greatly reduces walk speed.
        maxHealth += 75;
        health = maxHealth;
        if (walkSpeed > 4)
        {
            walkSpeed -= 4f;
        }
        else if (walkSpeed <= 4 && walkSpeed > 1)
        {
            walkSpeed = 1f;
        }
        else 
        {
            walkSpeed = 0.5f;
        }    
    }

    // -------Movement
    public void SqueezE()
    { // Increases walk speed but slightly reduces maximum health.
        maxHealth -= 7.5f;
        if (health > maxHealth - 7.5f) 
        {
            health = maxHealth;
        }
        walkSpeed += 1.5f;
    }

    public void CoolerDash() 
    { // Decreases cooldown time of dash (can be used maximum 3 times)
        dashCooldown -= 0.25f;
    }

    // -------Combat
    public void ExtraBarrel()
    {
        

    }

    public void PressurizedBullets()
    { // Doubles the speed of bullets
        bulletSpeedMultiplier *= 2;
    }

    public void FatalBullets() 
    { // Increases the damage of bullets
        bulletSpeedMultiplier *= 1.5f;
    }

    public void HighCaliber()
    { // Greatly increases the damage of bullets but also increases the time between shots
        if (gun == 0)
        {
            timeBetweenAutomaticGunShots += 0.3f;           
        }
        else if (gun == 1)
        {
            timeBetweenRevolverShots += 0.4f;
        }
        else if (gun == 2) 
        {
            timeBetweenShotgunShots += 0.5f;
        }
        bulletDamageMultiplier *= 2.25f;
    }

    public void KatanaDash()
    { // Dashes give damage to the enemies (one time)
        unlockedKatanaDash = true;
    }
}

