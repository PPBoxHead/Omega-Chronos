using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    #region Variables
    private static ParticlePoolManager instance;
    #region Setup
    [SerializeField] private List<GameObject> particlesPool;
    [SerializeField] private GameObject particlesPrefab;
    private Transform particlesParent;
    private int particlesAmount = 10;
    #endregion
    #endregion

    #region Methods
    private void Awake()
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

    private void Start()
    {
        particlesParent = GameObject.Find("ParticlePool").transform;
        particlesPool = new List<GameObject>();

        GameObject go;

        for (int i = 0; i < particlesAmount; i++)
        {
            go = Instantiate(particlesPrefab);
            go.transform.parent = particlesParent;
            go.SetActive(false);

            particlesPool.Add(go);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < particlesAmount; i++)
        {
            if (!particlesPool[i].activeInHierarchy)
            {
                return particlesPool[i];
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        if (instance != this)
        {
            instance = null;
        }
    }
    #endregion

    #region Setter/Getter
    public static ParticlePoolManager GetInstance
    {
        get { return instance; }
    }
    #endregion
}
