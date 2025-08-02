using System.IO;
using UnityEngine;

namespace PG.BattleSystem
{
    public class Statistics : MonoBehaviour
    {
        public static Statistics instance;


        [SerializeField] private string _fileName = "Statistics.json";
        public string filepath => Path.Combine(Application.persistentDataPath, _fileName);

        [SerializeField] private int _level = 1;
        public int level
        {
            get => _level;
            set
            {
                _level = value;
                levelChanged?.Invoke(_level);
            }
        }
        public event System.Action<int> levelChanged;
        public float exp
        {
            get => _exp;
            set
            {
                _exp = value;
                expChanged?.Invoke(_exp);
            }
        }
        [SerializeField] private float _exp;
        public event System.Action<float> expChanged;
        [field: SerializeField] public float startExpLevel { get; private set; } = 30f;
        [field: SerializeField] public float addExpLevel { get; private set; } = 20f;
        public float maxExpLevel => startExpLevel + (_level * addExpLevel);

        [System.Serializable]
        public class SaveStatistics
        {
            public int level;
            public float exp;
        }

        private SaveStatistics _saveStatistics = new SaveStatistics();


        private void Awake()
        {
            if (instance != this && instance != null)
            {
                Destroy(gameObject);
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Load();
        }
        public void Save()
        {
            _saveStatistics.level = level;
            _saveStatistics.exp = exp;
            string json = JsonUtility.ToJson(_saveStatistics);
            File.WriteAllText(filepath, json);
        }
        public void Load()
        {
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath); 
                JsonUtility.FromJsonOverwrite(json, _saveStatistics);
                level = _saveStatistics.level;
                exp = _saveStatistics.exp;
            }
        }
    }
}
