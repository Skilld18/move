using UnityEngine;

public class Move : MonoBehaviour
{
    public static int range = 110;
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.forward * (10 * Time.deltaTime));
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(camera.transform.position);
        var islands = GameObject.FindGameObjectsWithTag("island");
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float angle = 360f;
        var controls = transform.right * -x + transform.up * -z;
        controls.Normalize();
        var targetIsland = islands[0];
        foreach (var island in islands)
        {
            float dist = Vector3.Distance(island.transform.position, transform.position);
            var dir = island.transform.position - transform.position;
            dir.Normalize();
            if (dist <= range)
            {
                island.GetComponent<Renderer> ().material.color = Color.green;
                Debug.DrawLine(transform.position, island.transform.position);
                if (Vector3.Angle(dir, controls) < angle)
                {
                    angle = Vector3.Angle(dir, controls);
                    targetIsland = island;
                }
            }
        }
        Debug.DrawLine(transform.position, targetIsland.transform.position, Color.cyan);
    }
}
