using UnityEngine;

public class Island : MonoBehaviour
{
    private void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > Move.Range)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
