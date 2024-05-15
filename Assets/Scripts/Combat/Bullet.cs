using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;
    Vector3 direction;
    GameObject parent;
    public bool IsBoomerang;
    bool moveback = false;
    float aliveTime = 0;

    private void FixedUpdate()
    {
        if (!moveback)
        {
            Move();
        }
        else
        {
            Backwards();
        }
    }

    private void Update()
    {
        DestroyBullet();
        aliveTime += Time.deltaTime;
    }

    void Move()
    {
        direction.z = 0;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += direction * speed * Time.deltaTime;
    }

    void Backwards()
    {
        direction.z = 0;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += direction * -speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Unwalkable"))
        {
            Destroy(gameObject);
        }
        if (parent.CompareTag("Player") && other.CompareTag("Enemy"))
        {
            if (IsBoomerang)
            {
                int hitsTaken = 0;
                hitsTaken += 1;
                Enemy enemy = other.GetComponent<Enemy>();
                Debug.Log("Hit Enemy");
                enemy.TakeDamage(damage);
                if (hitsTaken == 2)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
                Enemy enemy = other.GetComponent<Enemy>();
                Debug.Log("Hit Enemy");
                enemy.TakeDamage(damage);
            }
        }
        else if ((parent.CompareTag("Enemy") || parent is null) && other.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerController.player.TakeDamage(damage);
        }
    }

    public static Bullet Create(Vector3 position, Quaternion rotation, Vector3 direction, float damage, float speed, GameObject parent)
    {
        // Instantiate a new bullet object
        GameObject bulletObject = new GameObject("Bullet");
        Bullet bullet = bulletObject.AddComponent<Bullet>();

        // Set bullet properties
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.direction = direction;
        bullet.parent = parent;
        bullet.damage = damage;
        bullet.speed = speed;

        return bullet;
    }

    public bool CheckForDestroy()
    {
        if (aliveTime < 10)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }


    void DestroyBullet()
    {
        if (CheckForDestroy() && !IsBoomerang)
        {
            Destroy(gameObject);
        }
        else if (CheckForDestroy() && IsBoomerang && moveback && Vector2.Distance(parent.transform.position, transform.position) >= 11f)
        {
            Destroy(gameObject);
        }
        else if (CheckForDestroy() && IsBoomerang)
        {
            moveback = true;
        }
    }

    public float GetSpeed() 
    {
        return speed;
    }
    public void SetSpeed(float speed) 
    {
        this.speed = speed;
    }
    public float GetDamage() 
    {
        return damage;
    }
    public void SetDamage(float damage) 
    {
        this.damage = damage;
    }
    public Vector3 GetDirection() 
    {
        return direction;
    }
    public void SetDirection(Vector3 direction) 
    {
        this.direction = direction;
    }
    public GameObject getParent() 
    {
        return parent;
    }
    public void SetParent(GameObject parent) 
    {
        this.parent = parent;
    }

}
