using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int createOnStart;

    private List<GameObject> pooledObjs = new List<GameObject>();

    void Start()
    {
        for (int x = 0; x < createOnStart; x++) {
            CreateNewBullet();
        }
    }

    GameObject CreateNewBullet()
    {
        GameObject bulletObj = Instantiate(bulletPrefab);

        bulletObj.SetActive(false);    //this is how we say this object is no longer in the world

        pooledObjs.Add(bulletObj);
        
        return bulletObj;
    }
    
    public GameObject GetObject()
    {
        GameObject obj = obj = pooledObjs.Find(x => x.activeInHierarchy == false); //the .Find works kinda like a for loop as in it will keep running looking for (in this case) objects that are now destroyed

        if (obj == null) {
            obj = CreateNewBullet();
        }
        obj.SetActive(true);

        return obj;
    }
}