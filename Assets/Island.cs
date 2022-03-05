using UnityEngine;

public class Island : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > Move.range)
        {
            GetComponent<Renderer> ().material.color = Color.red;
        }
    }
}
