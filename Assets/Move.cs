using UnityEngine;

public class Move : MonoBehaviour
{
    public static int range = 110;

    private GameObject jumpTarget = null;
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
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
            if (island == jumpTarget)
            {
                continue;
            }
            targetIsland.GetComponent<Renderer> ().material.color = Color.blue;
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
        targetIsland.GetComponent<Renderer> ().material.color = Color.magenta;
        
        if (Input.GetKeyDown("joystick button 0"))
        {
            jumpTarget = targetIsland;
        }

        if (jumpTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpTarget.transform.position, Time.deltaTime * 100);
            if (Vector3.Distance(jumpTarget.transform.position, transform.position) < 0.1f)
            {
                jumpTarget.GetComponent<Renderer> ().material.color = Color.red;
            }
        }
    }
}
