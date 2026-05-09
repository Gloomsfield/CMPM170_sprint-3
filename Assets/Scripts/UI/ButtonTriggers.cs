using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ButtonTriggers : MonoBehaviour
    {
        [SerializeField] private int titleSceneIndex;
        [SerializeField] private int playSceneIndex;
        [SerializeField] private int creditsSceneIndex;

        public void StartTitleScene()
        {
            SceneManager.LoadScene(titleSceneIndex);
        }

        public void StartPlayScene()
        {
            SceneManager.LoadScene(playSceneIndex);
        }

        public void StartCreditsScene()
        {
            SceneManager.LoadScene(creditsSceneIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
    }
}