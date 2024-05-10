using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    // Components
    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        // Initialize components and variables
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        maxHealth = 20;
        health = 20;
        damage = 4;
        moveSpeed = 2f;
        canShoot = false;
        damageRadius = 2f;
        timeBetweenMeleeAttacks = 1.5f;
    }

    
}
