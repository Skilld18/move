using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public const int Range = 70;

    public static GameObject jumpTarget;
    private GameObject lastIsland;
    public InputActionAsset input;
    private bool canJump = true;
    public float epsilon = 10f;
    
    public static bool hitOrb = false;

    public static InputAction fireAction;
    public static InputAction lookAction;
    public static InputAction moveAction;

    public static int stage = 0;
    

    public Toggle zenMode;
    public Toggle waitTilLand;

    public InputActionMap gameplayActions;
    public static int jumpCount = 0;
    
    void Start()
    {
        input.Enable();
        fireAction = input.FindAction("Player/Fire");
        lookAction = input.FindAction("Player/Look");
        moveAction = input.FindAction("Player/Move");
        fireAction.Enable();
        lookAction.Enable();
        moveAction.Enable();
    }

    private void returnToIsland()
    {
        if (hitOrb)
        {
            transform.position = Vector3.MoveTowards(transform.position, lastIsland.transform.position, 300 * Time.deltaTime);
        }

        if (lastIsland && Vector3.Distance(lastIsland.transform.position, transform.position) < 0.1f)
        {
            hitOrb = false;
        }
    }

    private void Update()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(mainCamera.transform.position);
        var islands = GameObject.FindGameObjectsWithTag("island");
        var angle = 360f;
        var local = transform;
        var right = local.right;
        var up = local.up;
        var controls = -right * moveAction.ReadValue<Vector2>().x + up * moveAction.ReadValue<Vector2>().y;

        Utils.DestroyLines();
        returnToIsland();
        if (hitOrb)
        {
            return;
        }
        controls.Normalize();
        var targetIsland = islands[0];
        bool foundTargets = false;
        foreach (var island in islands)
        {
            var islandTransform = island.transform;
            var dir = islandTransform.position - local.position;
            dir = Vector3.ProjectOnPlane(dir, local.forward);
            dir.Normalize();
            if (island == jumpTarget)
            {
                continue;
            }

            if (!Utils.canCameraSee(island) && !zenMode.GetComponent<Toggle>().isOn)
            {
                continue;
            }
            if (Utils.inRange(island))
            {
                foundTargets = true;
                if (Vector3.Angle(dir, controls) < angle)
                {
                    angle = Vector3.Angle(dir, controls);
                    targetIsland = island;
                }
            }
        }
        if (foundTargets)
        {
            Utils.DrawLine(transform.position, targetIsland.transform.position, Color.green);
            targetIsland.GetComponent<Renderer> ().material.color = Color.green;
        }

        foreach (var island in islands)
        {
            if (island != targetIsland && (island.name == "Orb(Clone)" || (island.name == "Door(Clone)")))
            {
                Utils.DrawLine(local.position, island.transform.position, Color.cyan);
                continue;
            }
            if (island != targetIsland)
            {
                if (Utils.inRange(island) && island != targetIsland)
                {
                    if (Utils.canCameraSee(island))
                    {
                        island.GetComponent<Renderer>().material.color = Color.yellow;
                        Utils.DrawLine(local.position, island.transform.position, Color.yellow);
                    }
                    else
                    {
                        if (zenMode.GetComponent<Toggle>().isOn)
                        {
                            Utils.DrawLine(local.position, island.transform.position, Color.red);
                        }
                    }
                }
                else
                {
                    island.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }

        
        if (fireAction.triggered && foundTargets && (canJump || !waitTilLand.GetComponent<Toggle>().isOn))
        {
            jumpTarget = targetIsland;
            canJump = false;
            jumpCount++;
        }

        if (jumpTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpTarget.transform.position, Time.deltaTime * 100);
            if (Vector3.Distance(jumpTarget.transform.position, transform.position) < epsilon)
            {
                if (jumpTarget.name == "island(Clone)")
                {
                    lastIsland = jumpTarget;
                }
                jumpTarget.GetComponent<Renderer> ().material.color = Color.green;
                canJump = true;
            }
        }

        CameraMove._currentX += lookAction.ReadValue<Vector2>().x * Time.deltaTime * 400f;
        CameraMove._currentY += -lookAction.ReadValue<Vector2>().y * Time.deltaTime * 400f;
    }
}
