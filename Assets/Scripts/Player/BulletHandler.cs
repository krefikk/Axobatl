using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BulletHandler : MonoBehaviour
{
    float bulletSpeed = 10f;
    float damage = 2f;
    Vector3 facingDirection;

    private void Awake()
    {
        float radians = PlayerController.player.GetRotationAngle() * Mathf.Deg2Rad;
        facingDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
        facingDirection.Normalize();
        facingDirection.z = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        DestroyBullet();
    }

    void Move()
    {
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += facingDirection * bulletSpeed * Time.deltaTime;
    }

    public bool CheckForDestroy() 
    {
        // Checks if bullet is out of screen or hit an enemy
        if (Vector2.Distance(PlayerController.player.transform.position, transform.position) >= 10f)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    void DestroyBullet() 
    {
        if (CheckForDestroy()) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy is not null)
            {
                enemy.TakeDamage(damage);
            }
            else 
            {
                Debug.Log("Enemy is null");
            }
        }
        else if (collision.gameObject.CompareTag("Unwalkable"))
        {
            Destroy(gameObject);
        }
    }

    public void setDamage(float value) 
    { // Modify the damage of the bullet
        damage = value;
    }

    public float getDamage() { return damage; }

    public void setDirection(Vector3 direction) 
    {
        facingDirection.x = direction.x;
        facingDirection.y = direction.y;
    }

    public void setDirection(Vector2 direction)
    {
        facingDirection.x = direction.x;
        facingDirection.y = direction.y;
    }

    public Vector2 getDirection()
    {
        return facingDirection;
    }

}
