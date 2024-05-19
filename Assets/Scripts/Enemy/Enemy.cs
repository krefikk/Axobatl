using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{ // Mother (Super) Class of All Enemies

    [Header("Environment")]
    public Camera screenCamera;
    
    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Health")]
    public float maxHealth;
    public float health;
    public Slider healthBar;

    [Header("Movement")]
    public float moveSpeed;
    NavMeshAgent agent;
    public LayerMask unwalkableLayer;

    [Header("Combat")]
    float damage = 5;
    public float damageRadius; // Only for melee attacker enemies
    public float timeBetweenMeleeAttacks;
    float timeSinceLastMeleeAttack = 0;

    [Header("Necromancer")]
    public bool canNecromance;
    public Transform[] corners;
    public GameObject specificEnemyPrefab;
    float lastTimeSinceNecromanced = 0;
    float timeBetweenEachNecromance = 15f;
    public Transform[] necromancePoints;
    bool destinationSet = false;
    
    [Header("Ranged Combat")]
    int gun; // 0 represents auto gun, 1 represents revolver, 2 represents shotgun
    float timeBetweenAutomaticGunShots = 0.2f;
    float timeBetweenRevolverShots = 0.5f;
    float timeBetweenShotgunShots = 0.6f;
    float timeSinceLastShoot;
    public Transform rayThrower;

    [Header("Flags")]
    public bool canShoot = true;
    bool moving = false;
    public bool meleeEnemy = false;


    [Header("HUD")]
    public Vector3 healthBarOffset;

    // Components
    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        // Handling melee, necromancing and shooting flags
        if (!meleeEnemy)
        {
            if (canNecromance) { canShoot = false; meleeEnemy = true; damage = 10; }
            if (canShoot) { canNecromance = false; meleeEnemy = false; }
        }
        else 
        {
            canShoot = false;
        }
        // Initializing components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // Setting idle animations
        if (meleeEnemy && !canNecromance)
        {
            anim.SetInteger("IdleState", 0);
        }
        else if (canShoot)
        {
            anim.SetInteger("IdleState", 1);
        }
        else if (canNecromance) 
        {
            anim.SetInteger("IdleState", 2);
        }
        // Initializing AI
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        // Randomly choosing a gun, according to player's decision
        if (PlayerController.player.GetGun() == 0) { gun = Random.Range(1, 3); }
        else if (PlayerController.player.GetGun() == 1)
        {
            int indicator = Random.Range(0, 2);
            if (indicator == 1) { gun = 0; }
            else { gun = 2; }
        }
        else { gun = Random.Range(0, 2); }
    }

    private void Update()
    {
        if (!GameManager.gameManager.gamePaussed) 
        {
            DisplayHealth();
            FaceToPosition(PlayerController.player.transform.position);
            Necromance();
            if (canShoot && !CheckForObstacles())
            {
                Shoot();
            }
            else if (!canShoot)
            {
                GiveDamage();
            }
        }
             
    }

    private void FixedUpdate()
    {
        if (!GameManager.gameManager.gamePaussed) 
        {
            CalculateStoppingDistance();
            MoveToPlayer();
        }       
    }

    void CalculateStoppingDistance() 
    {
        if (!canShoot && !canNecromance)
        {
            // If enemy is a melee attacking enemy, move it to the player's position
            agent.stoppingDistance = 0.25f;
        }
        else if (canShoot)
        { // If enemy is a shooting enemy
            if (CheckForObstacles())
            { // If there is a obstacle between them, come closer to the player
                agent.stoppingDistance = 0.25f;
            }
            else
            { // If there is no obstacles between them, leave a distance between them
                agent.stoppingDistance = 8f;
            }
        }
        else if (canNecromance) 
        {
            if (Vector2.Distance(PlayerController.player.transform.position, transform.position) <= 8f) 
            {
                MoveAwayFromPlayer();
            }
        }
    }

    void MoveToPlayer() // For melee enemies which tries to reach the player to kill them
    {
        if (!canNecromance)
        {
            agent.SetDestination(PlayerController.player.transform.position);
        }   
    }

    void MoveAwayFromPlayer() 
    {
        float distance = 1;
        Transform farest = corners[0];
        if (!destinationSet)
        {   
            foreach (Transform corner in corners)
            { // Finds farest corner point of the map
                if (Vector2.Distance(corner.position, PlayerController.player.transform.position) >= distance && ((PlayerController.player.transform.position.y <= transform.position.y && PlayerController.player.transform.position.y >= corner.position.y) || (PlayerController.player.transform.position.y >= transform.position.y && PlayerController.player.transform.position.y <= corner.position.y)))
                {
                    distance = Vector2.Distance(corner.position, transform.position);
                    farest = corner;
                }
            }
            if (Vector2.Distance(PlayerController.player.transform.position, transform.position) <= 12f)
            { // Moves away from player (goes to the closest corner point) if player is too close to it
                agent.SetDestination(farest.position);
                destinationSet = true;
            }
        }
        else 
        {
            if (transform.position == farest.position) 
            {
                destinationSet = false;
            }
        }
    }

    void FaceToPosition(Vector3 pos) // Faces enemy to desired position
    {
        Vector3 direction = pos - transform.position;
        direction.z = 0f; // Ensure the direction is in the XY plane
        direction.Normalize(); // Normalize the direction vector to have a magnitude of 1

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the character to face the mouse direction
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FaceToDirection(Vector3 direction) // Faces enemy to desired direction
    {
        direction.z = 0f; // Ensure the direction is in the XY plane
        direction.Normalize(); // Normalize the direction vector to have a magnitude of 1

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the character to face the mouse direction
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    bool CheckForObstacles()
    { // Checks if there a re any obstacles between enemy and player
        Vector3 direction = PlayerController.player.transform.position - transform.position;
        direction.z = 0f; // Ensure the direction is in the XY plane
        RaycastHit2D hit = Physics2D.Raycast(rayThrower.transform.position, direction, direction.magnitude);
        Debug.DrawRay(transform.position, direction);
        // Check if the ray hits an object
        if (hit.collider != null)
        {
            // If the hit object is on the "Unwalkable" layer, return true
            return hit.collider.CompareTag("Unwalkable");
            
        }
        // If no obstacles are detected, return false
        return false;
    }

    public void DisplayHealth()
    {
        healthBar.value = health / maxHealth;
        healthBar.transform.parent.rotation = screenCamera.transform.rotation;
        healthBar.transform.position = transform.position + healthBarOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { // If player has unlocked Katana Dash perk, enemies take damage while player dashing close enough to them
        if (collision.gameObject.CompareTag("Player") && PlayerController.player.getKatanaDashStatus() && PlayerController.player.isDashing())
        {
            TakeDamage(3);
        }
    }

    public void TakeDamage(float damage)
    {
        // If damage taken is greater than enemy's current health points, kill the enemy
        if (damage >= health)
        {
            Destroy(gameObject);
            // Play some animations or sounds here
        }
        // If damage is not enough to kill the enemy, just decrease their health
        else
        {
            health -= damage;
            // Play some animations or sounds here
        }
    }

    public void Die() 
    {
    }

    void GiveDamage()
    {
        timeSinceLastMeleeAttack += Time.deltaTime;
        if (Vector2.Distance(PlayerController.player.transform.position, transform.position) <= damageRadius && !canShoot && timeSinceLastMeleeAttack >= timeBetweenMeleeAttacks)
        { // For melee attacker enemies
            anim.SetTrigger("Attacking");
            PlayerController.player.TakeDamage(damage);
            timeSinceLastMeleeAttack = 0;
        }       
    }

    public void Shoot() 
    {
        if (gun == 0)
        { // If gun is automatic gun
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (timeSinceLastShoot >= timeBetweenAutomaticGunShots)
            {
                ShootAutoGunBullet();
                timeSinceLastShoot = 0;
            }
        }
        else if (gun == 1)
        { // If gun is revolver
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (timeSinceLastShoot >= timeBetweenRevolverShots)
            {
                ShootRevolverBullet();
                timeSinceLastShoot = 0;
            }
        }
        else if (gun == 2)
        { // If gun is shotgun
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (timeSinceLastShoot >= timeBetweenShotgunShots)
            {
                ShootShotgunBullets();
                timeSinceLastShoot = 0;
            }
        }
        anim.SetTrigger("Attacking");
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
            bullet.SetSpeed(10f);
            bullet.SetDamage(15f);
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
            bullet.SetSpeed(10f);
            bullet.SetDamage(20f);
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
                bullet.SetSpeed(10f);
                bullet.SetDamage(15f);
                bullet.SetParent(gameObject);
            }
        }
    }

    public void Necromance() 
    {
        if (canNecromance) 
        {
            lastTimeSinceNecromanced += Time.deltaTime;
            if (lastTimeSinceNecromanced >= timeBetweenEachNecromance) 
            {
                anim.SetBool("Necromancing", true);
                foreach (Transform necromancePoint in necromancePoints) 
                {
                    GameObject newEnemy = Instantiate(specificEnemyPrefab, necromancePoint.position, Quaternion.identity);
                    Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
                    newEnemyScript.canNecromance = false;
                    newEnemyScript.health = newEnemyScript.maxHealth;
                    int luck = Random.Range(0, 3);
                    if (luck == 0 || luck == 1) 
                    { // Necromanced enemies have 33% chance to be shootable enemies
                        newEnemyScript.canShoot = false;
                    }
                    else 
                    {
                        newEnemyScript.canShoot = true;
                    }
                }
                lastTimeSinceNecromanced = 0;
                anim.SetBool("Necromancing", false);
            }
        }
    }

    public float GetRotationAngle() // Returns the enemy's current rotation value as "degrees"
    {
        return transform.rotation.eulerAngles.z;
    }

    public Vector3 GetDirection() // Returns the ENEMY'S current look direction as a vector
    {
        float radians = GetRotationAngle() * Mathf.Deg2Rad;
        Vector3 facingDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        facingDirection.Normalize();
        return facingDirection;
    }
}
