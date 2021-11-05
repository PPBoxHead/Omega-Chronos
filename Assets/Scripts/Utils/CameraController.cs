using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;

    public void ChangeFocus(Transform newFocus)
    {
        cam.m_Follow = newFocus;
    }
}
