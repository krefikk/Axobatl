using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BulletHandler : MonoBehaviour
{
    float bulletSpeed = 10f;
    Vector3 facingDirection;

    private void Start()
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

    void Move()
    {
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += facingDirection * bulletSpeed * Time.deltaTime;
    }

}
