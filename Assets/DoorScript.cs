using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private const float WaitTime = 15f;
    private float _initTime;

    private void Start()
    {
        RenderSettings.skybox = (Material) Resources.Load("SkySeries Freebie/6sidedCosmicCoolCloud");
    }

    private void Update()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        var player = Utils.GetPlayer();
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            if (Move.stage < 4)
            {
                Move.stage++;
            }
        }
        MoveDoor();
    }

    private void MoveDoor()
    {
        if (!(Time.time - _initTime > WaitTime) || Move.stage >= 4)
        {
            return;
        }
        var randomVector = Utils.RandomVector(Cam.Scale);
        transform.position = randomVector;
        _initTime = Time.time;
    }
}
