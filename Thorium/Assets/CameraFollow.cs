using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private bool smoothCam = false;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothSpeed;
    [SerializeField]
    private Vector3 offset;

    private void LateUpdate()
    {
        if (smoothCam)
        {
            //camera follows player with duration
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            //camera follows player in the same time
            transform.position = target.position + offset;
        }
        
    }
}
