using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public const int Range = 70;

    public GameObject jumpTarget;
    public InputActionAsset input;
    private bool canJump = true;
    public float epsilon = 10f;
    public static InputAction fireAction;
    public static InputAction lookAction;
    public static InputAction moveAction;
    

    public Toggle zenMode;
    public Toggle waitTilLand;

    public InputActionMap gameplayActions;
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
        
        // Utils.DrawLine(local.position, local.position + (controls * 2), Color.red);
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
        }

        if (jumpTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, jumpTarget.transform.position, Time.deltaTime * 100);
            if (Vector3.Distance(jumpTarget.transform.position, transform.position) < epsilon)
            {
                jumpTarget.GetComponent<Renderer> ().material.color = Color.green;
                canJump = true;
            }
        }

        CameraMove._currentX += lookAction.ReadValue<Vector2>().x * Time.deltaTime * 400f;
        CameraMove._currentY += -lookAction.ReadValue<Vector2>().y * Time.deltaTime * 400f;
    }
}
