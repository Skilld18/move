using UnityEngine;

public class Cam : MonoBehaviour
{
    public static int Scale = 200;
    public GameObject island;
    public int numIslands = 100;
    public GameObject orb;
    public int numOrbs;
    private void Start()
    {
        ProgGen();
    }

    private void ProgGen()
    {
        var x = 0f;
        var y = 0f;
        var z = 0f;
        GameObject islandInstance = island;
        for (var i = 0; i < numIslands; i++)
        {
            x = Random.value * Scale;
            y = Random.value * Scale;
            z = Random.value * Scale;
            islandInstance = Instantiate(island, new Vector3(x, y, z), Quaternion.identity);
            
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(x, y, z);
        Move.jumpTarget = islandInstance;
    
        
        for (var i = 0; i < numOrbs; i++)
        {
            x = Random.value * Scale;
            y = Random.value * Scale;
            z = Random.value * Scale;
            Instantiate(orb, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}
