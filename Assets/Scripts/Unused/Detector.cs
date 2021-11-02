using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private LineRenderer lightRay;
    private Transform[] points;
    private void Awake()
    {
        lightRay = GetComponent<LineRenderer>();
    }
    public void SetUpLine(Transform[] points)
    {
        lightRay.positionCount = points.Length;
        this.points = points;
    }
    private void FixedUpdate()
    {
        for (int i= 0; i< points.Length; i++)
        {
            lightRay.SetPosition(i, points[i].position);
        }
    }
}
