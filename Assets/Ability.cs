using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    float nextAbilitytime = 0f;
    public float everyXseconds = 10f;
    public bool wechseln = false;

    
    public IDictionary<string, bool> abilities = new Dictionary<string, bool>();
    public bool superjump = false;
    public bool doublejump = true;
    public bool gun = true;
    public bool rocketlauncher = false;


    void Start()
    {
        abilities.Add("Gun",true);
        abilities.Add("Doublejump",true);
        abilities.Add("Superjump",false);
        abilities.Add("Rocketlauncher",false);
        abilities["Superjump"] = superjump;
        abilities["Doublejump"] = doublejump;
        abilities["Gun"] = gun;
        abilities["Rocketlauncher"] = rocketlauncher;
    }

    // Update is called once per frame
    void Update()
    {   
        if(wechseln){
            if(Time.time >= nextAbilitytime){
                bool s = abilities["Superjump"];//voruebergehtend
                //alle sollen auf false gestellt werden (hat nicht geklapt)
                
                abilities["Superjump"] = false;
                abilities["Doublejump"] = false;
                abilities["Gun"] = false;
                abilities["Rocketlauncher"] = false;

                //zufaellige 
                if(s){
                    abilities["Doublejump"] = true;
                }else{
                    abilities["Superjump"] = true;
                }
                
                nextAbilitytime = Time.time + everyXseconds;
            }
        }else{
            abilities["Superjump"] = superjump;
            abilities["Doublejump"] = doublejump;
            abilities["Gun"] = gun;
            abilities["Rocketlauncher"] = rocketlauncher;
        }
    }

    public bool isAktive(string faehigkeit){
        if(abilities.ContainsKey(faehigkeit)){
            if(abilities[faehigkeit]){
                return true;
            }
        }
        return false;
        
    }

        
            // Gun,
            // Doublejump,
            // Superjump,
            // Flamephrower,
            // Rocketlauncher
}
