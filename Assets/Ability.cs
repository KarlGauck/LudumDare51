using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{
    float nextAbilitytime = 0f;
    public float everyXseconds = 10f;
    public bool wechseln = false;

    
    public IDictionary<string, bool> abilities = new Dictionary<string, bool>();
    public IDictionary<string, string> abilityClass = new Dictionary<string, string>();
    Tuple<string, string, int> vorherig = Tuple.Create(" ", " ", 0);

    void Start()
    {
        abilities.Add("Gun",true);
        abilities.Add("Doublejump",true);
        abilities.Add("Run",true);
        abilities.Add("Superjump",true);
        abilities.Add("Rocketlauncher",true);
        abilities.Add("Grenade",true);
        abilities.Add("Dash",true);
        abilityClass.Add("Gun","weapon");
        abilityClass.Add("Rocketlauncher","weapon");
        abilityClass.Add("Grenade","weapon");
        abilityClass.Add("Doublejump","movement");
        abilityClass.Add("Superjump","movement");
        abilityClass.Add("Run","movement");
        abilityClass.Add("Dash","movement");
    }

    // Update is called once per frame
    void Update()
    {   
        if(wechseln){
            if(Time.time >= nextAbilitytime){

                //alle false
                ICollection<string> keys = abilities.Keys;
                List<string> keys2 = new List<string>();
                foreach(var key in keys){
                    keys2.Add(key);
                }
                foreach(var  key in keys2){
                    if(abilities[key]){
                        int anzahl = 0;
                        if(abilityClass[key] == vorherig.Item2){
                            anzahl = vorherig.Item3 + 1;
                        }
                        vorherig = Tuple.Create(key, abilityClass[key], anzahl);
                    }
                    abilities[key] = false;
                }

                // eine zufällige
                string akey = randomAbility(keys2);
                abilities[akey] = true;
                print(true);
                print(akey);
                
                nextAbilitytime = Time.time + everyXseconds;
            }
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

    string randomAbility(List<string> keys){
        string key = " ";
        System.Random rd = new System.Random();
        for(int i = 0; i < 10; i++){
            int rand_num = rd.Next(0,keys.Count);
            key = keys[rand_num];
            if(vorherig.Item2 != abilityClass[key] ||  vorherig.Item3 < 2){
                if(vorherig.Item1 != key){
                    return key;
                }
            }
        }
        return key;
            
    }

            // Gun,
            // Doublejump,
            // Superjump,
            // Flamephrower,
            // Rocketlauncher
}
