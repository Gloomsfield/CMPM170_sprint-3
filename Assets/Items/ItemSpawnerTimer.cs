using System.Collections;
using UnityEngine;

namespace Items
{
    public class ItemSpawnerTimer : MonoBehaviour {
    
        [Header("Stats Settings")]
        [Header("Toggles spawning. Interactable by other scripts too.")] public bool spawningActive = true;
    
        [Header("Min-Max delays between spawning items.")]
        [SerializeField] int secondsDelayMinimum = 10;
        [SerializeField] int secondsDelayMaximum = 20;
        
        // TODO field for spawning zones. maybe make objects children of those spawn zones

        [SerializeField] [Header("Dummy number, should changing limit become a game mechanic?")] int itemLimit = 42;

        [SerializeField] [Header("List of prefabs for this spawner to instantiate.")] GameObject[] itemPrefabs;
    
        [SerializeField] [Header("Parent object for fabricated instances.")] GameObject spawnedItemsHolder;

        public void Start()
        {
            // Begin cycle of spawning objects until 
            SpawnItem(spawningActive);
        }

        private void SpawnItem(bool scheduleNext = true)
        {
            Debug.Log("TODO: Spawn Item");
        
            if (scheduleNext)
            {
                StartCoroutine(nameof(BeginDelayedItemSpawn));
            }
        }

        public IEnumerator BeginDelayedItemSpawn()
        {
            yield return new WaitForSeconds(Random.Range(secondsDelayMinimum, secondsDelayMaximum));

            SpawnItem(spawningActive);
        } 
    
    }
}