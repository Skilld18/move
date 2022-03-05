using UnityEngine;

public class Move : MonoBehaviour
{
    public static int range = 50;
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        var islands = GameObject.FindGameObjectsWithTag("island");
        foreach (var island in islands)
        {
            float dist = Vector3.Distance(island.transform.position, transform.position);
            if (dist <= range)
            {
                island.GetComponent<Renderer> ().material.color = Color.green;
                Debug.DrawLine(transform.position, island.transform.position);
            }
            
        }
    }
}
