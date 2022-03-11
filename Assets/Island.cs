using UnityEngine;

public class Island : MonoBehaviour
{
    private void Update()
    {
        var player = Utils.GetPlayer();
        var dist = Vector3.Distance(player.transform.position, transform.position);
        GetComponent<Renderer>().material.color = dist > Move.Range ? Color.black : Color.yellow;
        if (Move.TargetIsland == gameObject)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
