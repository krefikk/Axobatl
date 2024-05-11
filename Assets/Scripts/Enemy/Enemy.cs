using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ // Mother (Super) Class of All Enemies
    [Header("Health")]
    public float maxHealth;
    public float health;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Combat")]
    public float damage;
    public float damageRadius; // Only for melee attacker enemies
    public float timeBetweenMeleeAttacks;
    public float timeSinceLastMeleeAttack = 0;

    [Header("Flags")]
    public bool canShoot;
    public bool moving = false;

    private void Update()
    {
        GiveDamage();
    }

    private void FixedUpdate()
    {
        if (!canShoot)
        {
            // If enemy is a melee attacking enemy, move it to the player's position
            MoveToPlayer();
        }
    }

    void MoveToPlayer() // For melee enemies which tries to reach the player to kill them
    {
        Vector2 direction = PlayerController.player.transform.position - transform.position;
        direction.Normalize();
        FaceToDirection(direction); // Faces enemy to the player
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        transform.position += movement;
        // moving animation and maybe sound

        if (movement.magnitude > 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
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
            PlayerController.player.TakeDamage(damage);
            timeSinceLastMeleeAttack = 0;
        }
        else if (canShoot)
        { // For ranged enemies
            
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
