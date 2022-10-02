using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Ability ability;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public GameObject grenadePrefab;
    public float fireRate = 2f;
    float nextAtacktime = 0f;
    public float fireRateGun = 5f;
    public float fireRateRocket = 1f;
    public float fireRateGrenade = 0.5f;

    
    void Update()
    {
        if(ability.isAktive("Gun")){
			fireRate = fireRateGun;
		 }else if(ability.isAktive("Rocketlauncher")){
			fireRate = fireRateRocket;
		}else if(ability.isAktive("Grenade")){
			fireRate = fireRateGrenade;
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
			Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
		}else if(ability.isAktive("Grenade")){
			Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
		}
        
    }
}
