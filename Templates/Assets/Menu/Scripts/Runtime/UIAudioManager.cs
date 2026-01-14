using UnityEngine;

namespace PG.MenuManagement
{
    // Этот компонент вешаем на ОДИН объект на сцене (например, на Canvas или пустой объект "AudioManager")
    public class UIAudioManager : MonoBehaviour
    {
        public static UIAudioManager Instance;

        [Header("Global UI Sounds")]
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _clickClip;
        [SerializeField] private AudioClip _selectClip;

        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            // Простейшая реализация Синглтона
            if (Instance == null)
            {
                Instance = this;
                // Не удалять при переходе между сценами (опционально)
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

        private void Play(AudioClip clip)
        {
            if (clip != null && _audioSource != null)
            {
                // PlayOneShot позволяет звукам накладываться (быстрые клики)
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}