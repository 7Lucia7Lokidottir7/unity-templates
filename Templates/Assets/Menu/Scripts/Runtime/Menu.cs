using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.MenuManagement
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private bool _cursorVisibleOnAwake;
        [SerializeField] private MenuSceneManager _menuSceneManager;
        [SerializeField] private string[] _savefiles;
        public static bool CursorVeisible {  get; private set; }
        string GetSaveFilePath(int index)
        {
             return Path.Combine(Application.persistentDataPath, _savefiles[index]);
        }
        // Start is called before the first frame update
        void Awake()
        {
            OnChangeCursorVisible(_cursorVisibleOnAwake);
        }
        public void NewGame(string sceneID)
        {
            if (_menuSceneManager != null)
            {
                _menuSceneManager.LoadScene(sceneID);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneID);
            }
            ClearSave();
        }
        public void ClearSave()
        {
            for (int i = 0; i < _savefiles.Length; i++)
            {
                if (File.Exists(GetSaveFilePath(i)))
                {
                    File.Delete(GetSaveFilePath(i));
                }
            }
        }
        public void Continue(string sceneID)
        {
            if (_menuSceneManager != null)
            {
                _menuSceneManager.LoadScene(sceneID);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneID);
            }
        }
        public void Restart()
        {
            OnChangeCursorVisible(false);
            Time.timeScale = 1.0f;
            if (_menuSceneManager != null)
            {
                _menuSceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
        }
        public void OnQuit()
        {
            Application.Quit();
        }
        public static void OnChangeCursorVisible(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
            CursorVeisible = visible; // Обновляем значение переменной CursorVeisible
        }

    }

}
