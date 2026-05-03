using System.Collections;
using System.Linq;
using UnityEngine;

namespace Items
{
    public class SpawnExecutor : MonoBehaviour {

        [SerializeField] [Header("Toggles spawning. Interactable by other scripts too.")] bool spawningActive = true;

        [Header("Min-Max delays between spawning items.")]
        [SerializeField] int secondsDelayMinimum = 10;
        [SerializeField] int secondsDelayMaximum = 20;

        // TODO field for spawning zones. maybe make objects children of those spawn zones

        [SerializeField] [Header("Dummy number, should changing limit become a game mechanic?")] int itemLimit = 42;

        public IEnumerator Start()
        {
            // Begin cycle of spawning items
            yield return SpawnItem();
        }

        private IEnumerator SpawnItem()
        {
            // Blocks Zone's SpawnItem() until
            yield return new WaitWhile(() => !(spawningActive && CountExistingItems() < itemLimit));

            // Actually spawn item
            GetSpawnZone().SpawnItem();

            // Block next spawn for the configured amount of time (set in editor)
            yield return new WaitForSeconds(Random.Range(secondsDelayMinimum, secondsDelayMaximum));
            yield return SpawnItem(); // And encore
        }

        private WeightedItemSpawnZone GetSpawnZone()
        {
            // get a spawn zone, any if more than 1
            WeightedItemSpawnZone[] spawnZones = transform.GetComponentsInChildren<WeightedItemSpawnZone>(false);
            return spawnZones[Random.Range(0, spawnZones.Length)];
        }

        private int CountExistingItems()
        {
            return transform.Cast<Transform>().Sum(childTransform => childTransform.childCount);
        }
    
    }
}