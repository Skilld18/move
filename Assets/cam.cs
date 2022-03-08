using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject island;
    // Start is called before the first frame update
    public int numIslands = 100;
    public int scale = 1000;
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
        for (var i = 0; i < numIslands; i++)
        {
            x = Random.value * scale;
            y = Random.value * scale;
            z = Random.value * scale;
            Instantiate(island, new Vector3(x, y, z), Quaternion.identity);
            
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(x, y, z);
    
        
        for (var i = 0; i < numOrbs; i++)
        {
            x = Random.value * scale;
            y = Random.value * scale;
            z = Random.value * scale;
            Instantiate(orb, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}
