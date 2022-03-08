using System.Linq;
using Unity.VisualScripting;
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
        var player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(player.transform.position, transform.position) < 1f)
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
