using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public IDictionary<string, bool> abilities = new Dictionary<string, bool>();

    //abilities.Add("Gun",true);

    void Start()
    {
        //abilities.Add("Gun",true);
        abilities.Add("Doublejump",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
            // Gun,
            // Doublejump,
            // Superjump,
            // Flamephrower,
            // Rocketlauncher
}
