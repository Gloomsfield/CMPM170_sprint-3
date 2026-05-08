using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public class PlaytestTimer : MonoBehaviour
    {
        [SerializeField] [Header("Allocated time for playtest, in seconds")] private int seconds = 60 * 5; // 5 minutes
        [SerializeField] [Header("Scene index to load once timer ends")] private int nextScene = 0;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            yield return new WaitForSeconds(seconds);
            SceneManager.LoadScene(nextScene);
        }

    }
}
