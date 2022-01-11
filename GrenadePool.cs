using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePool : MonoBehaviour
{
    public GameObject grenadePrefab;
    public int createOnStart;

    private List<GameObject> pooledObjs = new List<GameObject>();

    void Start()
    {
        for (int x = 0; x < createOnStart; x++) {
            CreateNewGrenade();
        }
    }

    GameObject CreateNewGrenade() 
    {
        GameObject grenadeObj = Instantiate(grenadePrefab);

        grenadeObj.SetActive(false);

        pooledObjs.Add(grenadeObj);

        return grenadeObj;
    }

    public GameObject GetObject()
    {
        GameObject obj = obj = pooledObjs.Find(x => x.activeInHierarchy == false); //the .Find works kinda like a for loop as in it will keep running looking for (in this case) objects that are now destroyed

        if (obj == null) {
            obj = CreateNewGrenade();
        }
        obj.SetActive(true);

        return obj;
    }
}
