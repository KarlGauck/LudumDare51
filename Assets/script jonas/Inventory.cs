using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> inventar;

    public void addItem(string a){
        if(!inventar.Contains(a)){
            inventar.Add(a);
        }
    }

    public bool hasItem(string a){
        return inventar.Contains(a);
    }
}
