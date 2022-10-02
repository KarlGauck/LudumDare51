using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Ability ability;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public float fireRate = 2f;
    float nextAtacktime = 0f;
    public float fireRateGun = 5f;
    public float fireRateRocket = 1f;

    
    void Update()
    {
        if(ability.isAktive("Gun")){
			fireRate = fireRateGun;
		 }else if(ability.isAktive("Rocketlauncher")){
			fireRate = fireRateRocket;
		}
        if(Time.time >= nextAtacktime){
            if(Input.GetButtonDown("Fire1")){
                Shoot();
                nextAtacktime = Time.time + 1f / fireRate;
            }
            
        }
    }
    void Shoot(){
        if(ability.isAktive("Gun")){
			Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		}else if(ability.isAktive("Rocketlauncher")){
            //Vector3 v = new Vector3(0.5,0,0)
			Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
		}
        
    }
}
