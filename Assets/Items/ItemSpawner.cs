using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemCategoryGroup
{
    public ItemCategory category; // What category this group is
    public int weight = 1; // How likely this category will be chosen to spawn an item
    public List<GameObject> prefabs = new List<GameObject>(); 
}

public class ItemSpawner : MonoBehaviour {

    // List of all categories. Each containing prefabs and weight
    [SerializeField] private List<ItemCategoryGroup> categoryGroups;

    // Spawning height
    private const int spawnHeight = 10; 
    // Bounds of spawning area
    private const int xMin = -5;
    private const int xMax = 5;
    private const int zMin = -5;
    private const int zMax = 5;

    // Increase the weight for a certain category
    // Idea is when player does x action with prefab from y category its weight gets increased (More likely to spawn similar prefab again)
    public void IncreaseWeight(ItemCategory category)
    {
        foreach (ItemCategoryGroup group in categoryGroups)
        {
            if(group.category == category)
            {
                group.weight++;
                return;
            }
        }
    }

    // Decrease the weight for a certain category
    public void DecreaseWeight(ItemCategoryGroup item)
    {
        // I dont know if we have a trash can or something so categories become less relevant 
    }

    // Main spawning function
    public void SpawnItem()
    {
        ItemCategoryGroup group = PickWeightedCategoryGroup();

        // Check if group is not valid
        if(group == null || group.prefabs.Count == 0)
        {
            return;
        }

        // Grabs a random prefab from that category
        int index = Random.Range(0, group.prefabs.Count);

        Instantiate(group.prefabs[index], MakeSpawnPosition(), Quaternion.identity);
    }

    // Picks a category based on weighted randomness
    /*
    Example:
        Bodies weight = 1
        Weapons weight = 1
        Tools weight = 1

        Total weight = 3

        Random number can be 0, 1, 2

        Bodies =  0
        Weapons = 1
        Tools = 2

        Each category has an equal chance

        Later if Weapons were interacted a lot they would become more likely

        Bodies weight = 1
        Weapons weight = 4
        Tools weight = 1

        Total weight = 6

        Bodies =  0
        Weapons = 1, 2, 3, 4
        Tools = 5

        Weapoins is much more likely to be picked
    */
    private ItemCategoryGroup PickWeightedCategoryGroup()
    {
        int totalWeight = 0;

        // Add all weights
        foreach (ItemCategoryGroup group in categoryGroups)
        {
            totalWeight += group.weight;
        }

        // Pick a random number in the range
        int random = Random.Range(0, totalWeight);

        int current = 0;

        // Find which category the random number falls into
        foreach (ItemCategoryGroup group in categoryGroups)
        {
            current += group.weight;

            if(random < current)
            {
                return group;
            }

        }

        return null;

    }

    //Helper function to random get x and z coordinates for spawning
    private Vector3 MakeSpawnPosition()
    {
        return new Vector3(Random.Range(xMin, xMax), spawnHeight, Random.Range(zMin, zMax));
    }
}
