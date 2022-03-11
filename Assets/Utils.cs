using UnityEngine;

public static class Utils
{
    private static float fudge = 1.2f;
    public static bool CanCameraSee(GameObject o)
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera.GetComponent<Camera>());
        return GeometryUtility.TestPlanesAABB(planes, o.GetComponent<Collider>().bounds);
    }

    public static bool InRange(GameObject o)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var dist = Vector3.Distance(player.transform.position, o.transform.position);
        if (o.name == "Orb(Clone)")
        {
            dist /= fudge;
        }
        return dist < Move.Range;
    }

    public static void DestroyLines()
    {
        var lines = GameObject.FindGameObjectsWithTag("line");
        foreach (var line in lines)
        {
            Object.Destroy(line);
        }
    }


    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.endWidth = 0.01f;
        lr.startWidth = 0.01f;
        lr.material.color = color;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        myLine.tag = "line";
        
        
        var cyl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // cyl.AddComponent<MeshRenderer>();
        cyl.GetComponent<MeshRenderer>().material.color = color;
        cyl.transform.localEulerAngles = new Vector3(90, 0, 0);
        cyl.transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(end, start));
        cyl.transform.position = Vector3.Lerp(start, end, 0.5f);
        cyl.tag = "line";
        if (Vector3.Distance(end, start) > 0.1f)
        {
            cyl.transform.rotation = Quaternion.LookRotation(end - start);
        }
        else
        {
            Object.Destroy(cyl);
        }
    }
}