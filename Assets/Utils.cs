using System.Linq;
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

    public static GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
    
    public static GameObject[] GetIslands(bool includeObjectives)
    {
        return GameObject.FindGameObjectsWithTag("island").Where(x => x.name == "island(Clone)" || includeObjectives).ToArray();
    }

    public static Vector3 RandomVector(float scale = 1f)
    {
        return new Vector3(Random.value, Random.value, Random.value) * scale;
    }

    public static void DestroyLines()
    {
        DestroyAll(GameObject.FindGameObjectsWithTag("line"));
    }

    private static void DestroyAll(GameObject[] array)
    {
        array.ToList().ForEach(Object.Destroy);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        if (end == start)
        {
            return;
        }
        var line = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        line.GetComponent<MeshRenderer>().material.color = color;
        line.transform.localEulerAngles = new Vector3(90, 0, 0);
        line.transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(end, start));
        line.transform.position = Vector3.Lerp(start, end, 0.5f);
        line.transform.rotation = Quaternion.LookRotation(end - start);
        line.tag = "line";
    }
}