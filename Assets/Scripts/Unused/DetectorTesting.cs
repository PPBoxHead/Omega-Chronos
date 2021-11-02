using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTesting : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Detector line;

    private void Start()
    {
        line.SetUpLine(points);
    }
}
