using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public const int Range = 70;

    public static GameObject jumpTarget;
    private GameObject lastIsland;
    public InputActionAsset input;
    private bool canJump = true;
    public float epsilon = 17f;
    
    public static bool hitOrb = false;

    public static InputAction fireAction;
    public static InputAction lookAction;
    public static InputAction moveAction;

    public static int stage = 0;
    

    public Toggle zenMode;
    public Toggle waitTilLand;
    public Text victory;

    public InputActionMap gameplayActions;
    public static int jumpCount = 0;
    private float startTime = 0f;

    private void Start()
    {
        InitGame();
        InitInput();
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

    private int _oldStage = -1;
    private void paint()
    {
        switch (stage)
        {
            case 0:
                GetComponent<MeshRenderer>().material.color = Color.grey;
                break;
            case 1:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 2:
                GetComponent<MeshRenderer>().material.color = new Color(255f, 0f,255f);
                break;
            case 3:
                GetComponent<MeshRenderer>().material.color = Color.white;
                break;
        }

        if (_oldStage != Move.stage)
        {
            GetComponent<AudioSource>().clip = (AudioClip) Resources.Load(stage.ToString());
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }

        _oldStage = Move.stage;
    }

    private float _jumpHeld = 0;
    private void Update()
    {
        if (stage == 4)
        {
            victory.enabled = true;
            victory.text = "Victory!\nJumps: " + Move.jumpCount.ToString() + "\nTime: " + (Time.time - startTime).ToString("F2") + "\nHold jump to restart";
            stage++;
            return;
        }
        else if (stage == 5)
        {
            if (fireAction.IsPressed())
            {
                if (_jumpHeld + 4f < Time.time)
                {
                    InitGame();
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                _jumpHeld = Time.time;
            }
            
        }
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(mainCamera.transform.position);
        var islands = Utils.GetIslands(true);
        var angle = 360f;
        var local = transform;
        var right = local.right;
        var up = local.up;
        var controls = -right * moveAction.ReadValue<Vector2>().x + up * moveAction.ReadValue<Vector2>().y;

        Utils.DestroyLines();
        returnToIsland();
        paint();
        if (hitOrb)
        {
            return;
        }
        controls.Normalize();
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

        var targetIsland = islands.Count() > 0 ? islands[0] : new GameObject();
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

            if (!Utils.CanCameraSee(island) && !zenMode.GetComponent<Toggle>().isOn)
            {
                continue;
            }
            if (Utils.InRange(island))
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
                if (Utils.InRange(island) && island != targetIsland)
                {
                    if (Utils.CanCameraSee(island))
                    {
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
                    island.GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }

        
        if (fireAction.triggered && foundTargets && (canJump || !waitTilLand.GetComponent<Toggle>().isOn))
        {
            jumpTarget = targetIsland;
            canJump = false;
            jumpCount++;
        }

        HandleInput();
    }

    private void InitGame()
    {
        _oldStage = -1;
        stage = 0;
        hitOrb = false;
        startTime = Time.time;
        victory.enabled = false;
        jumpCount = 0;
        RenderSettings.skybox = (Material) Resources.Load("SkySeries Freebie/6sidedFluffball");
    }

    private void InitInput()
    {
        input.Enable();
        fireAction = input.FindAction("Player/Fire");
        lookAction = input.FindAction("Player/Look");
        moveAction = input.FindAction("Player/Move");
        fireAction.Enable();
        lookAction.Enable();
        moveAction.Enable();
    }

    private static void HandleInput()
    {
        var cameraInput = lookAction.ReadValue<Vector2>() * Time.deltaTime;
        var mouseTweak = CameraMove.Sensitivity;
        if (lookAction.triggered &&
            lookAction.activeControl.ToString().Contains("Mouse/delta"))
        {
            mouseTweak *= 0.1f;
        }
        CameraMove.CurrentY += -cameraInput.y * mouseTweak;
        CameraMove.CurrentX += cameraInput.x * mouseTweak;
    }
}