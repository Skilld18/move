using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject island;
    // Start is called before the first frame update
    public int numIslands = 100;
    public int scale = 1000; 
    void Start()
    {
        return;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        for (var i = 0; i < numIslands; i++)
        {
            x = Random.value * scale;
            y = Random.value * scale;
            z = Random.value * scale;
            Instantiate(island, new Vector3(x, y, z), Quaternion.identity);
            
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(x, y, z);
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
