using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{
    private ParticleSystem particles;
    public Vector3 direction;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        transform.right = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (particles.isStopped) gameObject.SetActive(false);
    }
}
