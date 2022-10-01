using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public Ability ausgeruestet = Ability.Superjump;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum Ability
        {
            Gun,
            Doublejump,
            Superjump,
            Flamephrower,
            Landed
        }
}
