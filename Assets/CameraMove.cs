using UnityEngine;

public class CameraMove : MonoBehaviour
{
 
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;
 
    public Transform lookAt;
 
    public float distance = 10.0f;
    private float _currentX;
    private float _currentY;
    public float sensitivity = 400.0f;


    // Update is called once per frame
    private void LateUpdate()
    {
 
        _currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        _currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
 
        _currentY = Mathf.Clamp(_currentY, YMin, YMax);
 
        var direction = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        var position = lookAt.position;
        transform.position = position + rotation * direction;
 
        transform.LookAt(position);
    }
}