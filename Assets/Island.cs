using UnityEngine;

public class Island : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > Move.Range)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }
}
