using UnityEngine;

public class Orb : MonoBehaviour
{
    public static float maxSpeed = 0.3f;
    public static float tooFar = 300;
    private float howLong = 3;
    private float startTime;
    private Vector3 randomVector;
    public GameObject door;

    void Update()
    {
        switch (Move.stage)
        {
            case 0:
                stage0();
                break;
            case 1:
                stage1();
                break;
            case 2:
                stage2();
                break;
            case 3:
                // stage3();
                break;
            
        }
        if (oob())
        {
            transform.position = new Vector3(30, 30, 30);
        }
    }

    private bool hitOrb()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(player.transform.position, transform.position) < 1f;
    }

    private void stage0()
    {
        if (hitOrb())
        {
            Move.stage++;
            Move.hitOrb = true;
            Destroy(this.gameObject);
        }

    }
    private void stage1()
    {
        if (hitOrb())
        {
            Move.stage++;
            Move.hitOrb = true;
            Destroy(this.gameObject);
        }
        transform.position += transform.forward * maxSpeed;
    }
    private void stage2()
    {
        if (hitOrb())
        {
            Move.stage++;
            Move.hitOrb = true;
            var pos = new Vector3(Random.value * Cam.scale, Random.value * Cam.scale,
                Random.value * Cam.scale);
            Instantiate(door, pos, Quaternion.identity);
            Destroy(this.gameObject);
        }
        
        if (startTime + howLong < Time.time)
        {
            startTime = Time.time;
            howLong = Random.value * 3;
            randomVector = new Vector3(Random.value, Random.value, Random.value) * (Random.value * maxSpeed);
        }
        else
        {
            transform.position += randomVector;
        }
    }
    private bool oob()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(player.transform.position, transform.position) > tooFar;
    }
}
