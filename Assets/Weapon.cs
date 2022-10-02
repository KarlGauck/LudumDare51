using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //public Abilities abilities;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public float fireRate = 2f;
    float nextAtacktime = 0f;
    public float fireRateGun = 5f;
    public float fireRateRocket = 1f;

    
    void Update()
    {
        // Switch(abilities.ausgeruestet){
        //     case Ability.Gun:
        //         fireRate = fireRateGun;
        //         break;
        //     case Ability.Rocketlauncher:
        //         fireRate = fireRateRocket;
        //         break;
        // }
        if(Time.time >= nextAtacktime){
            if(Input.GetButtonDown("Fire1")){
                Shoot();
                nextAtacktime = Time.time + 1f / fireRate;
            }
            
        }
    }
    void Shoot(){
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
