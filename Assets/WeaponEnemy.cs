using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 2f;
    float nextAtacktime = 0f;

    
    void Update()
    {
        if(Time.time >= nextAtacktime){
            Shoot();
            nextAtacktime = Time.time + 1f / fireRate;
        }
    }
    void Shoot(){
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}