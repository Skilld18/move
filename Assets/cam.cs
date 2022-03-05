using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject island;
    // Start is called before the first frame update
    public int numIslands = 100;
    public int scale = 1000; 
    void Start()
    {
        for (var i = 0; i < numIslands; i++)
        {
            float x = UnityEngine.Random.value * scale;
            float y = UnityEngine.Random.value * scale;
            float z = UnityEngine.Random.value * scale;
            Instantiate(island, new Vector3(x, y, z), Quaternion.identity);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
