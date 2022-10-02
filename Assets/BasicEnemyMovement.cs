using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicEnemyMovement : MonoBehaviour
{
    
    void Update()
    {
        // Cast different Rays
        for (int i = 0; i < 20; i++) 
        {
            double angle = (Math.PI / 21.0) * (i+1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));

            if (hit.collider == null)
                continue;

            
        }
    }
}
