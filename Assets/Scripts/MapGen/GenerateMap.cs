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

    void InstantiateRandomTile(int x, int y) {
        // Choose a random tile type based on some probabilities
        float random = Random.value;
        GameObject tilePrefab;
        if (random < 0.8) {
            // 80% chance of a normal tile
            tilePrefab = tilePrefabs[0];
        } else if (random < 0.9) {
            // 10% chance of a wall tile
            tilePrefab = tilePrefabs[1];
        } else {
            // 10% chance of a door tile
            tilePrefab = tilePrefabs[2];
        }

        Vector3 position = new Vector3(y, 0, x); // Swap x and y
        Instantiate(tilePrefab, position, Quaternion.identity);
    }
}