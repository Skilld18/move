using UnityEngine;

public class Utils
{
    private static float fudge = 1.2f;
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
        if (o.name == "Orb(Clone)")
        {
            dist /= fudge;
        }
        return dist < Move.Range;
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material.color = color;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
