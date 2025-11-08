
namespace PG.Menu.Editor
{
    using UnityEditor;
    using TMPro.EditorUtilities;

[CustomEditor(typeof(TMP_DropdownWithText))]
public class TMP_DropdownWithTextEditor : DropdownEditor
{
    private TMP_DropdownWithText _dropdown;

    protected override void OnEnable()
    {
        base.OnEnable();
        _dropdown = (TMP_DropdownWithText)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Text Settings", EditorStyles.boldLabel);

        SerializedProperty textColors = serializedObject.FindProperty("textColors");
        EditorGUILayout.PropertyField(textColors, true);

        serializedObject.ApplyModifiedProperties();
    }
}
}


