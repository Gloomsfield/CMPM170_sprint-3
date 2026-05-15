using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PauseController : MonoBehaviour
    {
        [Header("Name of scene to load when pausing the game")]
        [SerializeField] private string pauseSceneName = "PauseSceneOverlay";
        
        private bool asyncActive;
        private bool paused;
        
        private void OnPause(InputValue value)
        {
            StartCoroutine(nameof(TriggerPause));
        }

        private IEnumerator TriggerPause()
        {
            if (asyncActive) // Debounce, since loading a scene is async. This avoids race conditions
                yield break;
            
            if (paused)
                yield return DoUnpause();
            else
                yield return DoPause();
        }

        private IEnumerator DoUnpause()
        {
            asyncActive = true;

            var sceneAsync = SceneManager.UnloadSceneAsync(pauseSceneName);

            yield return new WaitUntil(() => sceneAsync?.isDone ?? true);
            
            asyncActive = false;
            paused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private IEnumerator DoPause()
        {
            asyncActive = true;
            
            var sceneAsync = SceneManager.LoadSceneAsync(pauseSceneName, LoadSceneMode.Additive);

            yield return new WaitUntil(() => sceneAsync?.isDone ?? true);
            
            asyncActive = false;

            if (sceneAsync == null)
                yield break;
            
            // TODO set time for currently active scene to zero to stop physics
            
            var newScene = SceneManager.GetSceneByName(pauseSceneName);
            if (newScene.IsValid())
            {
                paused = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SceneManager.SetActiveScene(newScene);
            }
        }
    }
}
