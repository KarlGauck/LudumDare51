using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public int iLevelToLoad;
    public string sLevelToLoad;
    public bool loadLevelByInt = false;
    public string requiredItem = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "player")
        {
            Inventory inv = collisionGameObject.GetComponent<Inventory>();
            if (inv.hasItem(requiredItem))
                LoadScene();
        }
    }

    void LoadScene()
    {
        if (loadLevelByInt)
        {
            SceneManager.LoadScene(iLevelToLoad);
        }
        else
        {
            Debug.Log("load:" + sLevelToLoad);
            SceneManager.LoadScene(sLevelToLoad);
        }
    }
}
