using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public IDictionary<string, bool> abilities = new Dictionary<string, bool>();
    public bool superjump = false;
    public bool doublejump = true;

    //abilities.Add("Gun",true);

    void Start()
    {
        //abilities.Add("Gun",true);
        abilities.Add("Doublejump",true);
        abilities.Add("Superjump",false);
    }

    // Update is called once per frame
    void Update()
    {
        abilities["Superjump"] = superjump;
        abilities["Doublejump"] = doublejump;
    }
        
            // Gun,
            // Doublejump,
            // Superjump,
            // Flamephrower,
            // Rocketlauncher
}