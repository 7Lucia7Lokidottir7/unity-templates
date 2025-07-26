using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PG.LocalizationSystem
{
    public class LocalizationSystem : MonoBehaviour
    {
        private Dictionary<string, string> localizedText = new Dictionary<string, string>();
        private const char Delimiter = ','; // ����������� ��� CSV
        public enum TableType { CSV, TSV }
        [SerializeField] private TableType _tableType = TableType.CSV;
        public TableType tableType => _tableType;

        [SerializeField] private TextAsset[] _localizationFiles; // ������ CSV ������
        [SerializeField] private string _language = "English";
        public string currentLanguage => _language;

        public static LocalizationSystem instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            LoadLocalization();
        }

        public void LoadLocalization()
        {
            localizedText.Clear();

            foreach (TextAsset csvFile in _localizationFiles)
            {
                if (csvFile == null)
                {
                    Debug.LogError($"���� �� ������ ����������� �� ������.");
                    continue;
                }

                using (StringReader reader = new StringReader(csvFile.text))
                {
                    string headerLine = reader.ReadLine(); // ���������� ���������
                    if (string.IsNullOrEmpty(headerLine)) continue;

                    string[] headers = new string[0];
                    switch (_tableType)
                    {
                        case TableType.CSV:
                            headers = headerLine.Split(Delimiter);
                            break;
                        case TableType.TSV:
                            headers = headerLine.Split("\t");
                            break;
                    }

                    int languageIndex = -1;
                    for (int i = 0; i < headers.Length; i++)
                    {
                        if (headers[i].Trim() == _language)
                        {
                            languageIndex = i;
                            break;
                        }
                    }

                    if (languageIndex == -1)
                    {
                        Debug.LogError($"���� '{_language}' �� ������ � ����� ����������� {csvFile.name}.");
                        continue;
                    }

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = ParseCSVLine(line);
                        if (fields.Length > languageIndex)
                        {
                            string key = fields[0];
                            string value = fields[languageIndex];

                            if (!localizedText.ContainsKey(key))
                            {
                                localizedText[key] = value;
                            }
                            else
                            {
                                Debug.LogWarning($"���� '{key}' �� ����� {csvFile.name} ��� ����������. ������������.");
                            }
                        }
                    }
                }
            }
        }

        private string[] ParseCSVLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string currentField = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                switch (_tableType)
                {
                    case TableType.CSV:
                        if (c == Delimiter && line[i + 1] == ' ')
                        {
                            // ����������� ��������� �������
                            inQuotes = true;
                        }
                        else if (c == Delimiter && line[i + 1] != ' ')
                        {
                            inQuotes = false;
                        }
                        break;
                    case TableType.TSV:
                        if (c == '\t')
                        {
                            inQuotes = !inQuotes;
                        }
                        break;
                }
                if (c == Delimiter && !inQuotes)
                {
                    // ����������� ��� ������� � ���������� ����
                    fields.Add(currentField.Trim());
                    currentField = "";
                }
                else
                {
                    // ��������� ������ � ������� ����
                    currentField += c;
                }
            }

            // ��������� ��������� ����, ���� ��� ����
            if (!string.IsNullOrEmpty(currentField))
            {
                fields.Add(currentField.Trim());
            }

            return fields.ToArray();
        }


        public string GetLocalizedValue(string key, string defaultValue = null)
        {
            if (localizedText.TryGetValue(key, out string value))
            {
                return value;
            }

            Debug.LogWarning($"���� '{key}' �� ������ � �����������.");
            return string.IsNullOrWhiteSpace(defaultValue) ? key : defaultValue; // ���������� ���� ��� fallback
        }
    }
}
