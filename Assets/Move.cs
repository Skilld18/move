using System.Collections.Generic;
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
    public static GameObject TargetIsland;
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

    private void Update()
    {
        RestartCheck();
        if (VictoryCheck())
        {
            return;
        }

        Utils.DestroyLines();
        ReturnToIsland();
        PaintAndMusic();
        if (HitOrb)
        {
            return;
        }
        MoveTowardJumpTarget();
        
        var islands = Utils.GetIslands(true);
        var foundTargets = FindJumpTargets(islands);
        DrawLines(islands);
        HandleJumpInput(foundTargets);
        HandleCameraInput();
    }

    private bool FindJumpTargets(IReadOnlyList<GameObject> islands)
    {
        var angle = 360f;
        TargetIsland = islands[0];
        var foundTargets = false;
        
        var local = transform;
        var right = local.right;
        var up = local.up;

        var controls = -right * _moveAction.ReadValue<Vector2>().x + up * _moveAction.ReadValue<Vector2>().y;
        controls.Normalize();
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
            if (!Utils.InRange(island))
            {
                continue;
            }
            if (!(Vector3.Angle(dir, controls) < angle))
            {
                continue;
            }
            foundTargets = true;
            angle = Vector3.Angle(dir, controls);
            TargetIsland = island;
        }
        return foundTargets;
    }

    private void HandleJumpInput(bool foundTargets)
    {
        Utils.DrawLine(transform.position, TargetIsland.transform.position, Color.green);
        if (!_fireAction.triggered || !foundTargets || (!_canJump && waitTilLand.GetComponent<Toggle>().isOn))
        {
            return;
        }
        JumpTarget = TargetIsland;
        _canJump = false;
        _jumpCount++;
    }

    private void DrawLines(IEnumerable<GameObject> islands)
    {
        var local = transform;
        foreach (var island in islands)
        {
            if (island != TargetIsland && (island.name == "Orb(Clone)" || (island.name == "Door(Clone)")))
            {
                Utils.DrawLine(local.position, island.transform.position, Color.cyan);
                continue;
            }
            if (!Utils.InRange(island) || island == TargetIsland)
            {
                continue;
            }
            if (Utils.CanCameraSee(island))
            {
                Utils.DrawLine(local.position, island.transform.position, Color.yellow);
            }
            else if (zenMode.GetComponent<Toggle>().isOn)
            {
                Utils.DrawLine(local.position, island.transform.position, Color.red);
            }
        }
    }

    private void MoveTowardJumpTarget()
    {
        if (!JumpTarget)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, JumpTarget.transform.position, Time.deltaTime * 100);
        if (!(Vector3.Distance(JumpTarget.transform.position, transform.position) < 17f))
        {
            return;
        }
        if (JumpTarget.name == "island(Clone)")
        {
            _lastIsland = JumpTarget;
        }
        _canJump = true;
    }
    private bool VictoryCheck()
    {
        if (Stage != 4)
        {
            return false;
        }
        victory.enabled = true;
        victory.text = "Victory!\nJumps: " + _jumpCount + "\nTime: " + (Time.time - _startTime).ToString("F2") + "\nHold jump to restart";
        Stage++;
        return true;
    }

    private float _jumpHeld;
    private void RestartCheck()
    {
        if (Stage != 5) return;
        if (_fireAction.IsPressed())
        {
            if (!(_jumpHeld + 4f < Time.time)) return;
            InitGame();
            SceneManager.LoadScene(0);
        }
        else
        {
            _jumpHeld = Time.time;
        }
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

    private void PaintAndMusic()
    {
        GetComponent<MeshRenderer>().material.color = Orb.Colors[Stage%4];
        if (_oldStage != Stage)
        {
            GetComponent<AudioSource>().clip = (AudioClip) Resources.Load(Stage.ToString());
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        _oldStage = Stage;
    }

    private void HandleCameraInput()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        transform.LookAt(mainCamera.transform.position);
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