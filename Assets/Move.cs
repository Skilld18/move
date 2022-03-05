using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static int range = 300;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 10 * Time.deltaTime);
        var islands = GameObject.FindGameObjectsWithTag("island");
        foreach (var island in islands)
        {
            island.GetComponent<Renderer> ().material.color = Color.green;
            
        }
    }
}
