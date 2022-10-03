using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
	public int health = 100;
	public HealthBar healthBar;

	float nextRegeneration = 0f;
    public float regenerationTime = 10f;
	public int regenerationStrenght = 10;

	public GameObject deathEffect;

	public void Start(){
		health = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}

	public void Update(){
		if(Time.time >= nextRegeneration){
            health += regenerationStrenght;
			if(health > maxHealth){
				health = maxHealth;
			}
			healthBar.SetHealth(health);
            nextRegeneration = Time.time + regenerationTime;
        }
	}

	public void TakeDamage (int damage)
	{
		health -= damage;
		healthBar.SetHealth(health);
		 
		if (health <= 0)
		{
			Die();
		}
	}

	void Die ()
	{
		if(deathEffect != null){
			Instantiate(deathEffect, transform.position, Quaternion.identity);
		}
		Destroy(gameObject);
	}
}
