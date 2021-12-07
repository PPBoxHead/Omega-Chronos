using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform laserDir;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform firePoint;

    public GameObject startParticles;
    public GameObject endParticles;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        FillList();//funcionando
        DisableLaser();
    }

    public void EnableLaser()
    {
        lineRenderer.enabled = true;

        for (int i = 0; i < particles.Count; i++)
            particles[i].Play();
    }

    public void UpdateLaser()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        startParticles.transform.position = firePoint.position;

        lineRenderer.SetPosition(1, laserDir.position);

        Vector2 direction = laserDir.position - firePoint.position;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction.normalized, direction.magnitude);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        endParticles.transform.position = lineRenderer.GetPosition(1);
    }

    public void DisableLaser()
    {
        lineRenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
            particles[i].Stop();
    }

    void FillList()
    {
        for (int i = 0; i < startParticles.transform.childCount; i++)
        {
            var ps = startParticles.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }

        for (int i = 0; i < endParticles.transform.childCount; i++)
        {
            var ps = endParticles.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
    }

}
