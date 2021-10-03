using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Honestly to myself, idk wtf does everything of this specifically, but it works.
    public Transform target;
    //public Vector3 offset; comentado porque en cameragod no es necesario, en las otras si
    public Camera mainCamera;

    [Range(1, 20)]
    [SerializeField] private float smoothFactor;

    private void FixedUpdate()
    {
        Follow();
        Vector3 camEndPosition = mainCamera.transform.position;

        LimitCamera(camEndPosition);
    }

    void Follow()
    {
        //Vector3 targetPosition = target.position + offset;
        Vector3 targetPosition = target.position;

        // Just makes the movement of the camera more smooth
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;

    }

    void LimitCamera(Vector3 camEndPosition)
    {
        if (camEndPosition.y > 3.5f)
        {
            mainCamera.transform.position = new Vector3(camEndPosition.x, 3.5f, camEndPosition.z);
        }
    }

}
