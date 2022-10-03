using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPoint : MonoBehaviour
{
    public string hinzufuegen = "Superjump";
    
    void OnTriggerEnter2D (Collider2D hitInfo)
	{
		Ability ability = hitInfo.GetComponent<Ability>();
		if (ability != null)
		{
			ability.addAbility(hinzufuegen);
            Destroy(gameObject);
		}

		//Instantiate(impactEffect, transform.position, transform.rotation);

		
	}
}
