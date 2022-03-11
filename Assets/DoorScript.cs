using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private float waitTime = 15f;
    private float _initTime = 0f;
    private float scale = 200f;

    void Update()
    {
        if (Move.jumpTarget != this)
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            if (Move.stage < 4)
            {
                Move.stage++;
            }
        }

        if (Time.time - _initTime > waitTime && Move.stage < 4)
        {
            var randomVector = new Vector3(Random.value, Random.value, Random.value) * scale;
            transform.position = randomVector;
            _initTime = Time.time;
        }
    }
}
