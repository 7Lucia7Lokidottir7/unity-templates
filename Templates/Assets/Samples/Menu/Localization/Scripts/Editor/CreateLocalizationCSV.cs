using PG.Localization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateLocalizationCSV
{
    [MenuItem("Tools/PG/Localization/Create CSV from object")]
    public static void GetAllText()
    {
        string filePath = EditorUtility.SaveFilePanel("Create Localization", Application.dataPath, "MenuLocalization", "csv");

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        LocalizeText[] localizeTexts = Selection.activeGameObject.GetComponentsInChildren<LocalizeText>(true);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Key,English,Russian,Ukrainian");
            foreach (var item in localizeTexts)
            {
                writer.WriteLine($@"{item.key},,,");
            }
            writer.Close();
        }
        AssetDatabase.Refresh();

    }
}
