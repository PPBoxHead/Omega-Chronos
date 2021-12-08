using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    #region Variables
    private static ParticlePoolManager instance;
    #region Setup
    [Header("Crash Particles")]
    [SerializeField] private List<GameObject> particlesPool;
    [SerializeField] private GameObject particlesPrefab;
    private int particlesAmount = 10;
    [Header("Explosion Particles")]
    [SerializeField] private List<GameObject> explosionPool;
    [SerializeField] private GameObject explosionsPrefab;
    private int explosionsAmount = 5;
    [Header("Smoke Particles")]
    [SerializeField] private List<GameObject> smokePool;
    [SerializeField] private GameObject smokesPrefab;
    private int smokesAmount = 5;
    private Transform particlesParent;
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
        // crash
        particlesParent = GameObject.Find("ParticlePool").transform;
        particlesPool = new List<GameObject>();
        explosionPool = new List<GameObject>();

        GameObject go;

        for (int i = 0; i < particlesAmount; i++)
        {
            go = Instantiate(particlesPrefab);
            go.transform.parent = particlesParent;
            go.SetActive(false);

            particlesPool.Add(go);
        }

        // explosions
        for (int i = 0; i < explosionsAmount; i++)
        {
            go = Instantiate(explosionsPrefab);
            go.transform.parent = particlesParent;
            go.SetActive(false);

            explosionPool.Add(go);
        }

        // Smoke
        for (int i = 0; i < smokesAmount; i++)
        {
            go = Instantiate(smokesPrefab);
            go.transform.parent = particlesParent;
            go.SetActive(false);

            smokePool.Add(go);
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

    public GameObject GetExplosion()
    {
        for (int i = 0; i < explosionsAmount; i++)
        {
            if (!explosionPool[i].activeInHierarchy)
            {
                return explosionPool[i];
            }
        }
        return null;
    }

    public GameObject GetSmoke()
    {
        for (int i = 0; i < smokesAmount; i++)
        {
            if (!smokePool[i].activeInHierarchy)
            {
                return smokePool[i];
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
