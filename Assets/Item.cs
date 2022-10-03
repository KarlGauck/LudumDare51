using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string item = "keycard";
    
    void OnTriggerEnter2D (Collider2D hitInfo)
	{
		Inventory inentar = hitInfo.GetComponent<Inventory>();
		if (inentar != null)
		{
			inentar.addItem(item);
            Destroy(gameObject);
		}

		//Instantiate(impactEffect, transform.position, transform.rotation);

		
	}
}
