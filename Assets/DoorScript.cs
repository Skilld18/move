using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private float waitTime = 15f;
    private float initTime = 0f;

    private void start()
    {
    }
    // Update is called once per frame
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

        if (Time.time - initTime > waitTime && Move.stage < 4)
        {
            var randomVector = new Vector3(Random.value, Random.value, Random.value) * Cam.scale;
            transform.position = randomVector;
            initTime = Time.time;
        }
    }
}
