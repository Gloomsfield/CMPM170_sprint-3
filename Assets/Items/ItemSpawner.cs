using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    // ALL ITEM PREFABS HELD HERE
    //[SerializeField] GameObject[] items;
    [SerializeField] private List<GameObject> items = new List<GameObject>();

    private const int spawnHeight = 10; //How High we want items to spawn
    private const int xMin = -5;
    private const int xMax = 5;
    private const int zMin = -5;
    private const int zMax = 5;

    public void SpawnItem(int index)
    {
        Instantiate(items[index], MakeSpawnPosition(), Quaternion.identity);
    }

    private Vector3 MakeSpawnPosition()
    {
        return new Vector3(Random.Range(xMin, xMax), spawnHeight, Random.Range(zMin, zMax));
    }
}
