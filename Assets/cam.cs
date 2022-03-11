using UnityEngine;

public class Cam : MonoBehaviour
{
    public int scale = 200;
    public GameObject island;
    public int numIslands = 100;
    public GameObject orb;
    public int numOrbs = 3;
    private void Start()
    {
        ProgGen();
    }
    
    private void ProgGen()
    {
        GenerateOrbs();
        GenerateIslands();
        MovePlayerToIsland();
    }

    private void GenerateOrbs()
    {
        Generate(orb, numOrbs);
    }
    private void GenerateIslands()
    {
        Generate(island, numIslands);
    }

    private void Generate(GameObject thing, int num)
    {
        for (var i = 0; i < num; i++)
        {
            Instantiate(thing, Utils.RandomVector(scale), Quaternion.identity);
        }
    }

    private static void MovePlayerToIsland()
    {
        var origin = Utils.GetIslands(false)[0];
        Utils.GetPlayer().transform.position = origin.transform.position;
        Move.jumpTarget = origin;
    }
}
