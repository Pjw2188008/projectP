using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public int maxHealth;
    public int health;

    public bool isDied;


    void Start()
    {
        health = maxHealth;

        isDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        isDied = true;
    }

    public void TakeGuard(int damage)
    {
        print("guard");

        health -= damage;

        if (health <= 0) Death();
    }

    public virtual void TakeDamage(int damage)
    {
        print("damage");

        health -= damage;

        if (health <= 0) Death();
    }
}
