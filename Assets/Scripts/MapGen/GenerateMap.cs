using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public int mapWidth = 10;
    public int mapHeight = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                InstantiateRandomTile(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateRandomTile(int x, int y)
    {
        int randomIndex = Random.Range(0, tilePrefabs.Length);
        GameObject randomTilePrefab = tilePrefabs[randomIndex];
        Vector3 position = new Vector3(x, 0, y);
        Instantiate(randomTilePrefab, position, Quaternion.identity);
    }
}
