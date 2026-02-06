using UnityEngine;
using UnityEngine.Audio; // Обязательно для AudioResource

namespace PG.MenuManagement
{
    public class UIAudioManager : MonoBehaviour
    {
        public static UIAudioManager Instance;

        [Header("Global UI Sounds")]
        // Используем AudioResource, как вы просили (для Random Containers и т.д.)
        [SerializeField] private AudioResource _hoverClip;
        [SerializeField] private AudioResource _clickClip;
        [SerializeField] private AudioResource _selectClip;

        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject); 
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void PlayHover() => Play(_hoverClip);
        public void PlayClick() => Play(_clickClip);
        public void PlaySelect() => Play(_selectClip);

        private void Play(AudioResource resource)
        {
            if (resource != null && _audioSource != null)
            {
                // ИСПРАВЛЕНИЕ:
                // AudioResource нельзя проиграть через PlayOneShot.
                // Нужно назначить его в свойство .resource и вызвать .Play()

                AudioSource audioSource = Instantiate(_audioSource);

                audioSource.resource = resource;
                audioSource.Play();


                if (resource is AudioClip clip)
                {
                    Destroy(audioSource.gameObject, clip.length);
                }
                else
                {
                    Destroy(audioSource.gameObject, 10f);
                }

            }
        }
    }
}