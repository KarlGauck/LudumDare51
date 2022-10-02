using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    public float speed = 20f;
    public float hight = 10f;
	public int damage = 70;
    public float explosionsRadius = 10f;
	public Rigidbody2D rb;
    public Transform attackPoint;
    public LayerMask explotionLayers;
    public float timeBeforExplotion = 2f;
    float expolotionTime = 0f;
	//public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		rb.velocity = transform.right * speed + transform.up * hight;
        expolotionTime = Time.time + timeBeforExplotion;
	}

    void Update(){
        if(Time.time >= expolotionTime){
            explode();
            
        }
    }

	void explode()
	{
		Destroy(gameObject);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, explosionsRadius, explotionLayers);
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            print("treffer");
            print(enemy);
        }
        // Enemy enemy = hitInfo.GetComponent<Enemy>();
		// if (enemy != null)
		// {
		// 	enemy.TakeDamage(damage);
		// }

		//Instantiate(impactEffect, transform.position, transform.rotation);
	}
	void OnDrawGizmosSelected(){
        if(attackPoint.position == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, explosionsRadius);
    }
}
