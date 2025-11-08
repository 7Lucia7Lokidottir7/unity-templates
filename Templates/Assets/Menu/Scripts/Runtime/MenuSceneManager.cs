using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.MenuManagement
{
    public class MenuSceneManager : MonoBehaviour
    {

        public virtual void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        public virtual void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

}
