using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform lookAt;
    public float distance = 10.0f;
    public static float CurrentX;
    public static float CurrentY;
    public const float Sensitivity = 400.0f;

    private void LateUpdate()
    {
        CurrentY = Mathf.Clamp(CurrentY, -50f, 50f);
        var direction = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
        var position = lookAt.position;
        transform.position = position + rotation * direction;
        transform.LookAt(position);
    }
}