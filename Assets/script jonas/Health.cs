using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
	public int health = 100;
	public HealthBar healthBar;

	public GameObject deathEffect;

	public void Start(){
		health = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
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
		if (deathEffect != null)
			Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
