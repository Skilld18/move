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
        var player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            print("Victory");
            print(Time.time);
            print(Move.jumpCount);
        }

        if (Time.time - initTime > waitTime)
        {
            var randomVector = new Vector3(Random.value, Random.value, Random.value) * Cam.scale;
            transform.position = randomVector;
            initTime = Time.time;
        }
    }
}
