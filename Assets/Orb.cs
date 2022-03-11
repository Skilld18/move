using UnityEngine;

public class Orb : MonoBehaviour
{
    private const float MaxSpeed = 0.3f;
    private float _howLong = 3;
    private float _startTime;
    private Vector3 _randomVector;
    public GameObject door;

    private readonly Color[] _colors =
    {
        new(255, 0, 0),
        new(255,0,255),
        new(255,255,255)
    };

    private void Update()
    {
        HitOrb();
        GetComponent<MeshRenderer>().material.color = _colors[Move.Stage];
        switch (Move.Stage)
        {
            case 0:
                Stage0();
                break;
            case 1:
                Stage1();
                break;
            case 2:
                Stage2();
                break;
        }
        if (OutOfBounds())
        {
            transform.position = new Vector3(30, 30, 30);
        }
    }

    private void HitOrb()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var hit = Vector3.Distance(player.transform.position, transform.position) < 1f;
        if (hit)
        {
            Move.Stage++;
            Move.HitOrb = true;
            if (Move.Stage == 1)
            {
                RenderSettings.skybox = (Material) Resources.Load("SkySeries Freebie/MegaSun");
            }
            if (Move.Stage == 2)
            {
                RenderSettings.skybox = (Material) Resources.Load("SkySeries Freebie/PlanetaryEarth");
            }
            if (Move.Stage == 3)
            {
                Instantiate(door, Utils.RandomVector(Cam.Scale), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private static void Stage0() { }
    private void Stage1()
    {
        var t = transform;
        t.position += t.forward * (MaxSpeed * Random.value);
    }
    private void Stage2()
    {
        if (_startTime + _howLong < Time.time)
        {
            _startTime = Time.time;
            _howLong = Random.value * 3;
            _randomVector = Utils.RandomVector(Random.value * MaxSpeed);
        }
        else
        {
            transform.position += _randomVector;
        }
    }
    private bool OutOfBounds()
    {
        var player = Utils.GetPlayer();
        return Vector3.Distance(player.transform.position, transform.position) > Cam.Scale;
    }
}