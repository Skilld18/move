using System.Linq;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public static float maxSpeed = 0.3f;
    public static float tooFar = 1000;
    private float howLong = 3;
    private float startTime;
    private Vector3 randomVector;
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
                stage3();
                break;
            
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
        }

    }
    private void stage1()
    {
            Move.stage++;
    }
    private void stage2()
    {
            Move.stage++;
    }
    private void stage3()
    {
        if (hitOrb())
        {
            if (GameObject.FindGameObjectsWithTag("island").Where(x => x.name == this.name).Count() <= 1)
            {
                Debug.Log("The enemies' gate is down");
            }
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
        
        if (oob())
        {
            transform.position = new Vector3(30, 30, 30);
        }
    }

    private bool oob()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(player.transform.position, transform.position) > tooFar;
    }
    
}
