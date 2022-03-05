using UnityEngine;

public class Move : MonoBehaviour
{
    public const int Range = 70;

    public GameObject jumpTarget;
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    private void Update()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(mainCamera.transform.position);
        var islands = GameObject.FindGameObjectsWithTag("island");
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var angle = 360f;
        var local = transform;
        var right = local.right;
        var up = local.up;
        var controls = right * -x + up * -z;
        
        Debug.DrawRay(local.position, controls, Color.red);
        controls.Normalize();
        var targetIsland = islands[0];
        foreach (var island in islands)
        {
            var dist = Vector3.Distance(island.transform.position, transform.position);
            var islandTransform = island.transform;
            var dir = islandTransform.position - local.position;
            dir = Vector3.ProjectOnPlane(dir, local.forward);
            dir.Normalize();
            if (island == jumpTarget)
            {
                continue;
            }
            targetIsland.GetComponent<Renderer> ().material.color = Color.blue;
            if (dist <= Range)
            {
                Debug.DrawLine(transform.position, island.transform.position);
                if (Vector3.Angle(dir, controls) < angle)
                {
                    angle = Vector3.Angle(dir, controls);
                    targetIsland = island;

                }
                island.GetComponent<Renderer> ().material.color = Color.green;
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
                jumpTarget.GetComponent<Renderer> ().material.color = Color.white;
            }
        }
    }
}
