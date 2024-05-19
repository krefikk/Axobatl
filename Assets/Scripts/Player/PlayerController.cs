using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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
    private float maxHealth = 150;
    private float health = 150;

    [Header("Movement")]
    private float xAxis = 0;
    private float yAxis = 0;
    public float walkSpeed = 5f;

    [Header("Cards")]
    public List<int> displayedOneTimeCardIDs = new List<int>();

    [Header("Flags")]
    bool attacking = false;
    bool dead = false;
    bool moving = false;
    bool dashing = false;
    public bool inCardsScene = false;
    // Attributes
    bool mirrorArmorActivated = false;

    [Header("Dash")]
    bool canDash = true;
    public bool DashAbility = true;
    float dashSpeed = 15f;
    float dashTime = 0.2f;
    float dashCooldown = 1f;
    float dashDistanceMultiplier = 1;

    [Header("Perks")]
    float bulletSpeedMultiplier = 1;
    float bulletDamageMultiplier = 1;
    bool unlockedKatanaDash = false;
    bool unlockedSawBullets = false;

    [Header("Attributes")]
    int attribute; // Represents which attribute player choosed: 0 represents mirror armor
    float attributeUsingTime = 0;

    [Header("Mirror Armor")]
    float mirrorArmorCooldown = 20f;
    float mirrorArmorTime = 5f;
    bool isMirrorArmorOnCooldown = false;

    [Header("HUD")]
    public Slider healthBar;
    public TextMeshProUGUI elapsedTime;
    public TextMeshProUGUI hudScore;
    TextMeshProUGUI waveText;
    public GameObject waveTextParent;
    public GameObject inGameMenu;

    [Header("Waves")]
    int inWave = 0;
    public bool finishedGame = false;
    bool displayedWave1Text = false;
    bool displayedWave2Text = false;
    bool displayedWave3Text = false;
    bool displayedWave4Text = false;
    bool displayedWave5Text = false;

    [Header("Score")]
    float highScore;
    float score = 0;
    public bool highScoreUpdated = false;

    // Components
    Rigidbody2D rb;
    Animator anim;

    // Audio
    public EventReference audioHandgun;
    public EventReference audioRevolver;

    public EventReference audioPlayerDeath;

    private void Awake()
    {
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        waveText = waveTextParent.GetComponent<TextMeshProUGUI>();
        waveText.text = "";
        anim.SetInteger("Gun", gun);
    }

    private void Update()
    {
        GetInputs();
        if (!GameManager.gameManager.gamePaussed)
        {
            inGameMenu.SetActive(false);
            FaceMouse();
            Shoot();
            DisplayHealth();
            DisplayScore();
            DisplayElapsedTime();
            FinishGame();
        }
        else 
        {
            inGameMenu.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.gameManager.gamePaussed)
        {
            if (dashing) { return; } // Movement functions won't get called if player is dashing
            Move();
            StartDash();
        }
    }

    void GetInputs() 
    {
        xAxis = Input.GetAxis("Horizontal"); // Checks if player pressed left, right or A, D
        yAxis = Input.GetAxis("Vertical"); // Checks if player pressed up, down or W, S
        attacking = Input.GetMouseButtonDown(0); // Checks if player pressed left mouse button
        if (Input.GetKeyDown(KeyCode.Escape)) // Stops the game if game is flowing, starts the game if game is stopped
        {
            GameManager.gameManager.gamePaussed = !GameManager.gameManager.gamePaussed;
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
                anim.SetTrigger("Attacking");
                ShootAutoGunBullet();
                timeSinceLastShoot = 0;

                RuntimeManager.PlayOneShot(audioHandgun);
            }
        }
        else if (gun == 1)
        { // If gun is revolver
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (attacking && timeSinceLastShoot >= timeBetweenRevolverShots)
            {
                anim.SetTrigger("Attacking");
                ShootRevolverBullet();
                timeSinceLastShoot = 0;

                RuntimeManager.PlayOneShot(audioRevolver);
            }
        }
        else if (gun == 2)
        { // If gun is shotgun
            timeSinceLastShoot += Time.deltaTime;
            // Checks if player pressed attack button, and enough time passed since last shoot
            if (attacking && timeSinceLastShoot >= timeBetweenShotgunShots)
            {
                anim.SetTrigger("Attacking");
                ShootShotgunBullets();
                timeSinceLastShoot = 0;
            }
        }
    }

    void ShootAutoGunBullet() // Shoots bullet
    {
        float offset = 0;
        if (GetDirection().y < 0 && GetDirection().x > 0) { offset = 0.5f; }
        else if (GetDirection().y < 0 && GetDirection().x < 0) { offset = 0.5f; }
        else if (GetDirection().y > 0 && GetDirection().x > 0) { offset = -0.5f; }
        else if (GetDirection().y > 0 && GetDirection().x < 0) { offset = -0.5f; }
        Vector3 bulletSpawnPosition = transform.position + GetDirection() + new Vector3(offset, 0, 0);
        Vector3 bulletDirection = GetDirection();

        GameObject bulletObject = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetDirection(bulletDirection.normalized);
            bullet.SetSpeed(10f * bulletSpeedMultiplier);
            bullet.SetDamage(2f * bulletDamageMultiplier);
            bullet.SetParent(gameObject);
            if (unlockedSawBullets) 
            {
                bullet.IsBoomerang = true;
            }
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
            if (unlockedSawBullets)
            {
                bullet.IsBoomerang = true;
            }
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
                if (unlockedSawBullets)
                {
                    bullet.IsBoomerang = true;
                }
            }
        }
    }

    public void DisplayHealth() 
    {
        healthBar.value = health / maxHealth;
    }

    public void DisplayElapsedTime() 
    {
        elapsedTime.text = GameManager.gameManager.GetFormattedElapsedTime();
    }

    public void DisplayScore() 
    {
        hudScore.text = score.ToString();
    }

    public void DisplayWaveText() 
    {
        StartCoroutine(DisplayWaveTextCO());
    }

    IEnumerator DisplayWaveTextCO() 
    {
        if (inWave == 1 && !displayedWave1Text) 
        {
            waveText.text = "WAVE 1";
        }
        else if (inWave == 2 && !displayedWave2Text)
        {
            waveText.text = "WAVE 2";
        }
        else if (inWave == 3 && !displayedWave3Text)
        {
            waveText.text = "WAVE 3";
        }
        else if (inWave == 4 && !displayedWave4Text)
        {
            waveText.text = "WAVE 4";
        }
        else if (inWave == 5 && !displayedWave5Text)
        {
            waveText.text = "WAVE 5";
        }
        yield return new WaitForSeconds(1f);
        waveText.text = "WAVE";
        yield return new WaitForSeconds(0.5f);
        waveText.text = "WAV";
        yield return new WaitForSeconds(0.25f);
        waveText.text = "WA";
        yield return new WaitForSeconds(0.25f);
        waveText.text = "W";
        yield return new WaitForSeconds(0.25f);
        waveText.text = "";
        switch (inWave) 
        {
            case 1:
                displayedWave1Text = true;
                break;
            case 2:
                displayedWave2Text = true;
                break;
            case 3:
                displayedWave3Text = true;
                break;
            case 4:
                displayedWave4Text = true;
                break;
            case 5:
                displayedWave5Text = true;
                break;
        }
    }

    public void Die() 
    {
        AudioManager.audioManager.EndMusic();
        RuntimeManager.PlayOneShot(audioPlayerDeath);

        if (score > highScore) 
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            highScoreUpdated = true;
        }       
        dead = true;
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("GotWave1Award", 0);
        PlayerPrefs.SetInt("GotWave2Award", 0);
        PlayerPrefs.SetInt("GotWave3Award", 0);
        PlayerPrefs.SetInt("GotWave4Award", 0);
        SceneManager.LoadScene("GameOver");
    }

    public void FinishGame() 
    {
        if (finishedGame) 
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetFloat("HighScore", highScore);
                highScoreUpdated = true;
            }
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("GotWave1Award", 0);
            PlayerPrefs.SetInt("GotWave2Award", 0);
            PlayerPrefs.SetInt("GotWave3Award", 0);
            PlayerPrefs.SetInt("GotWave4Award", 0);
            SceneManager.LoadScene("GameOver");
        }
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

    public void Restart() 
    {
        /*
        canDash = true;
        DashAbility = true;
        maxHealth = 50;
        health = maxHealth;
        inWave = 0;
        bulletSpeedMultiplier = 1;
        bulletDamageMultiplier = 1;
        unlockedKatanaDash = false;
        unlockedSawBullets = false;
        timeBetweenAutomaticGunShots = 0.2f;
        timeBetweenRevolverShots = 0.5f;
        timeBetweenShotgunShots = 0.6f;
        walkSpeed = 5f;
        displayedOneTimeCardIDs.Clear();
        dead = false;
        inCardsScene = false;
        dashSpeed = 15f;
        dashTime = 0.2f;
        dashCooldown = 1f;
        dashDistanceMultiplier = 1;
        // All attribute bools
        mirrorArmorCooldown = 20f;
        mirrorArmorTime = 5f;
        isMirrorArmorOnCooldown = false;
        score = 0;
        transform.position = new Vector2(0.3f, -0.1f);
        highScoreUpdated = false;
        finishedGame = false;
        gameObject.SetActive(true);
        */
        Destroy(gameObject);
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
        if (DashAbility)
        {
            canDash = false;
            dashing = true;
            // Trigger animation
            Vector2 direction = new Vector2(xAxis, yAxis);
            if (direction.magnitude == 0)
            {
                rb.velocity = GetDirection() * dashSpeed * dashDistanceMultiplier;
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
    public GameObject getPlayer()
    {
        return this.gameObject;
    }
    
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

    public bool isShooting()
    {
        return attacking;
    }

    public float getTimeSinceLastShot()
    {
        return timeSinceLastShoot;
    }

    public float getTimeBetweenShots(int gun)
    {
        switch(gun)
        {
            case 0:
                return timeBetweenAutomaticGunShots;

            case 1:
                return timeBetweenRevolverShots;

            case 2:
                return timeBetweenShotgunShots;

            default:
                return 0;
        }

    }
    public int GetWaveNumber() 
    { // Returns the number of the wave player currently dealing
        if (inCardsScene) { return -1; }
        return inWave;
    }

    public void IncreaseWaveNumber(int number) 
    {
        inWave += number;
        DisplayWaveText();
    }

    public float GetScore() 
    {
        return score;
    }

    public void SetScore(float value) 
    {
        score = value;
    }

    //--------------------------------Perks------------------------------------------
    // ------Health
    public void HealthBoost()
    { // Increases max health and refreshes the health     
        maxHealth += 30;
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

    public void PlotArmor()
    { // Increases your health greatly but you lose your dash (one time)
        maxHealth += 100;
        health = maxHealth;
        DashAbility = false;
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
    public void SpeedUp()
    { // Slightly increases walk speed
        walkSpeed += 0.75f;
    }
    public void DashBurger()
    { // Increases Dashtime but decreases walkSpeed (one time)
        dashTime += 0.4f;
        walkSpeed -= 0.75f;
    }

    public void CoolerDash()
    { // Decreases cooldown time of dash (can be used maximum 3 times)
        dashCooldown -= 0.25f;
        if (dashCooldown <= 0) 
        {
            dashCooldown = 0.2f;
        }
    }

    public void DashAirlines()
    { // Increases the speed and distance of the dash.
        dashDistanceMultiplier *= 1.5f;
    }

    // -------Combat
    public void ExtraBarrel()
    {

        
    }

    public void DashBlast()
    {// greatly upgrades your dash but greatly decreases your health
        dashTime += 1f;
        dashDistanceMultiplier *= 2f;
        maxHealth -= 15;     
        if (maxHealth <= 0)
        {
            maxHealth = 5;

        }
        health = maxHealth;
    }



    public void PressurizedBullets()
    { // Doubles the speed of bullets
        bulletSpeedMultiplier *= 2;
    }

    public void FatalBullets()
    { // Increases the damage of bullets
        bulletSpeedMultiplier *= 1.5f;
    }

    public void ClickClack()
    { // Decreases time between shots
        if (gun == 0)
        {
            timeBetweenAutomaticGunShots -= 0.1f;
        }
        else if (gun == 1)
        {
            timeBetweenRevolverShots -= 0.2f;
        }
        else if (gun == 2)
        {
            timeBetweenShotgunShots -= 0.15f;
        }
    }
    public void BlastGum()
    { // Decreases time between shots but also decreases damage (one time)
        if (gun == 0 && timeBetweenAutomaticGunShots > 0.3f)
        {
            timeBetweenAutomaticGunShots -= 0.3f;
        }
        else if (timeBetweenAutomaticGunShots <= 0.3f) 
        {
            timeBetweenAutomaticGunShots -= 0.05f;
        }
        else if (gun == 1)
        {
            timeBetweenRevolverShots -= 0.3f;
        }
        else if (gun == 2)
        {
            timeBetweenShotgunShots -= 0.2f;
        }
        bulletDamageMultiplier /= 1.7f;
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
            timeBetweenShotgunShots += 0.3f;
        }
        bulletDamageMultiplier *= 2.25f;
    }

    public void KatanaDash()
    { // Dashes give damage to the enemies (one time)
        unlockedKatanaDash = true;
    }
    public void SawBullets()
    { // Bullets come back to the player (one time)
        unlockedSawBullets = true;
    }

    public void LetItBeBullet() 
    { // Greatly decreases time between shots but also greatly decreases walk speed. (one time)
        if (gun == 0)
        {
            timeBetweenAutomaticGunShots -= 0.1f;
            if (timeBetweenAutomaticGunShots <= 0) { timeBetweenAutomaticGunShots = 0.05f; }
        }
        else if (gun == 1)
        {
            timeBetweenRevolverShots -= 0.25f;
            if (timeBetweenRevolverShots <= 0) { timeBetweenRevolverShots = 0.05f; }
        }
        else if (gun == 2)
        {
            timeBetweenShotgunShots -= 0.3f;
            if (timeBetweenShotgunShots <= 0) { timeBetweenShotgunShots = 0.05f; }
        }
        walkSpeed -= 2.5f;
        if (walkSpeed <= 0) { walkSpeed = 0.1f; }
    }
    
    public void AxoWaddle()
    { // Greatly increases walk speed and bullet damage but greatly increases time between shots. (one time)
        bulletDamageMultiplier *= 2f;
        timeBetweenAutomaticGunShots *= 2f;
        timeBetweenRevolverShots *= 2;
        timeBetweenShotgunShots *= 2;
        walkSpeed *= 2f;
        if (walkSpeed <= 0) { walkSpeed = 0.1f; }
    }


}

