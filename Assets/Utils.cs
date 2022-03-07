using UnityEngine;

public class Utils
{
    public static bool canCameraSee(GameObject o)
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera.GetComponent<Camera>());
        return GeometryUtility.TestPlanesAABB(planes, o.GetComponent<Collider>().bounds);
    }

    public static bool inRange(GameObject o)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var dist = Vector3.Distance(player.transform.position, o.transform.position);
        return dist < Move.Range;
    }
}
