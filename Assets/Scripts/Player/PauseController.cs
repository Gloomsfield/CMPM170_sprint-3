using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PauseController : MonoBehaviour
    {
        // Name of scene to load when pausing the game
        private const string playSceneName = "PlayScene";
        private const string pauseSceneName = "PauseSceneOverlay";
        
        private static bool asyncActive;
        private static bool paused;
        
        private void OnPause(InputValue value)
        {
            StartCoroutine(nameof(TriggerPause));
        }

        private IEnumerator TriggerPause()
        {
            if (asyncActive) // Debounce, since loading a scene is async. This avoids race conditions, effectively an atomic lock
                yield break;
            
            if (paused)
                yield return DoUnpause();
            else
                yield return DoPause();
        }

        private IEnumerator DoUnpause()
        {
            if (asyncActive)
                yield break;

            asyncActive = true;

            SceneManager.UnloadSceneAsync(pauseSceneName);

            // yield return new WaitUntil(() => sceneAsync?.isDone ?? true);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(playSceneName));

            paused = false;
            asyncActive = false;
        }

        private IEnumerator DoPause()
        {
            if (asyncActive)
                yield break;

            asyncActive = true;

            var sceneAsync = SceneManager.LoadSceneAsync(pauseSceneName, LoadSceneMode.Additive);

            yield return new WaitUntil(() => sceneAsync?.isDone ?? true);

            if (sceneAsync != null)
            {
                var newScene = SceneManager.GetSceneByName(pauseSceneName);
                if (newScene.IsValid())
                {
                    // TODO set time for currently active scene to zero to stop physics

                    paused = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    SceneManager.SetActiveScene(newScene);
                }
            }

            asyncActive = false;
        }

        public void Resume()
        {
            StartCoroutine(nameof(DoUnpause));
        }

        public void StartTitleScene()
        {
            Resume();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            SceneManager.LoadScene(0);
        }
    }
}
