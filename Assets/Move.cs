using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    //Controls
    public InputActionAsset input;
    private static InputAction _fireAction;
    private static InputAction _lookAction;
    private static InputAction _moveAction;
    public const int Range = 70;

    //State control    
    public static int Stage;
    private int _oldStage = -1;
    private bool _canJump = true;
    public static bool HitOrb;
    public static GameObject JumpTarget;
    private GameObject _lastIsland;

    //UI
    public Toggle zenMode;
    public Toggle waitTilLand;
    public Text victory;
    
    //Scoring 
    private static int _jumpCount;
    private float _startTime;

    private void Start()
    {
        InitGame();
        InitInput();
    }

    private void ReturnToIsland()
    {
        if (HitOrb)
        {
            transform.position = Vector3.MoveTowards(transform.position, _lastIsland.transform.position, 300 * Time.deltaTime);
        }

        if (_lastIsland && Vector3.Distance(_lastIsland.transform.position, transform.position) < 0.1f)
        {
            HitOrb = false;
        }
    }

    private void Paint()
    {
        switch (Stage)
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

        if (_oldStage != Stage)
        {
            GetComponent<AudioSource>().clip = (AudioClip) Resources.Load(Stage.ToString());
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }

        _oldStage = Stage;
    }

    private float _jumpHeld;
    private void Update()
    {
        if (Stage == 4)
        {
            victory.enabled = true;
            victory.text = "Victory!\nJumps: " + _jumpCount + "\nTime: " + (Time.time - _startTime).ToString("F2") + "\nHold jump to restart";
            Stage++;
            return;
        }
        if (Stage == 5)
        {
            if (_fireAction.IsPressed())
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
        var controls = -right * _moveAction.ReadValue<Vector2>().x + up * _moveAction.ReadValue<Vector2>().y;

        Utils.DestroyLines();
        ReturnToIsland();
        Paint();
        if (HitOrb)
        {
            return;
        }
        controls.Normalize();
        if (JumpTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, JumpTarget.transform.position, Time.deltaTime * 100);
            if (Vector3.Distance(JumpTarget.transform.position, transform.position) < 17f)
            {
                if (JumpTarget.name == "island(Clone)")
                {
                    _lastIsland = JumpTarget;
                }
                JumpTarget.GetComponent<Renderer> ().material.color = Color.green;
                _canJump = true;
            }
        }

        var targetIsland = islands.Any() ? islands[0] : new GameObject();
        bool foundTargets = false;
        foreach (var island in islands)
        {
            var islandTransform = island.transform;
            var dir = islandTransform.position - local.position;
            dir = Vector3.ProjectOnPlane(dir, local.forward);
            dir.Normalize();
            if (island == JumpTarget)
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

        
        if (_fireAction.triggered && foundTargets && (_canJump || !waitTilLand.GetComponent<Toggle>().isOn))
        {
            JumpTarget = targetIsland;
            _canJump = false;
            _jumpCount++;
        }

        HandleCameraInput();
    }

    private void InitGame()
    {
        _oldStage = -1;
        Stage = 0;
        HitOrb = false;
        _startTime = Time.time;
        victory.enabled = false;
        _jumpCount = 0;
        RenderSettings.skybox = (Material) Resources.Load("SkySeries Freebie/6sidedFluffball");
    }

    private void InitInput()
    {
        input.Enable();
        _fireAction = input.FindAction("Player/Fire");
        _lookAction = input.FindAction("Player/Look");
        _moveAction = input.FindAction("Player/Move");
        _fireAction.Enable();
        _lookAction.Enable();
        _moveAction.Enable();
    }

    private static void HandleCameraInput()
    {
        var cameraInput = _lookAction.ReadValue<Vector2>() * Time.deltaTime;
        var mouseTweak = CameraMove.Sensitivity;
        if (_lookAction.triggered &&
            _lookAction.activeControl.ToString().Contains("Mouse/delta"))
        {
            mouseTweak *= 0.1f;
        }
        CameraMove.CurrentY += -cameraInput.y * mouseTweak;
        CameraMove.CurrentX += cameraInput.x * mouseTweak;
    }
}