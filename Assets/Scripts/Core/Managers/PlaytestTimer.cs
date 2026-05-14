using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public class PlaytestTimer : MonoBehaviour
    {
        [SerializeField] [Header("Allocated time for playtest, in seconds")] private int seconds = 60 * 5; // 5 minutes
       // [SerializeField] [Header("Scene index to load once timer ends")] private int nextScene = 3;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        IEnumerator Start()
        {
            yield return new WaitForSeconds(seconds);
            EventManager.invokePlaytestEnded(); // Fire this event so that listeners can replace message in GameManager.playtestResultMessage
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
