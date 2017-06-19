using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{


    protected Dictionary<string, GameObject> GameObjectResources;

    private void Awake()
    {
        LoadResources();
    }

    private void LoadResources()
    {
        // GameObjectResources
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("");
        GameObjectResources = new Dictionary<string, GameObject>();
        foreach(GameObject g in gameObjects)
        {
            GameObjectResources.Add(g.name, g);
        }
    }

    #region Methods for Getting things from dictionaries

    public GameObject GetResourcePrefab(string s)
    {
        if (GameObjectResources.ContainsKey(s) == false)
            throw new ArgumentException(s + " is not in GameObjectResources");
        return GameObjectResources[s];
    }

    #endregion
}

