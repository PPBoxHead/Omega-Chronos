using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    private static BulletPoolManager instance;
    private Transform bulletsParent;
    [SerializeField] private GameObject bulletPrefab;
    private float timer = 0;

    private int enemiesAmount = 20;

    [SerializeField] private List<GameObject> bulletsPool;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        bulletsParent = GameObject.Find("BulletPool").transform;
        bulletsPool = new List<GameObject>();

        GameObject go;

        for (int i = 0; i < enemiesAmount; i++)
        {
            go = Instantiate(bulletPrefab);
            go.transform.parent = bulletsParent;
            go.SetActive(false);

            bulletsPool.Add(go);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < enemiesAmount; i++)
        {
            if (!bulletsPool[i].activeInHierarchy)
            {
                return bulletsPool[i];
            }
        }
        return null;
    }

    void OnDestroy()
    {
        if (instance != this)
        {
            instance = null;
        }
    }

    public static BulletPoolManager GetInstance
    {
        get { return instance; }
    }
}
