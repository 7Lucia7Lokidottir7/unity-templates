using TMPro;

namespace PG.Menu.Editor
{
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(ButtonWithText))]
public class ButtonWithTextEditor : ButtonEditor
{
    private ButtonWithText _btn;

    protected override void OnEnable()
    {
        base.OnEnable();
        _btn = (ButtonWithText)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Text Settings", EditorStyles.boldLabel);

        _btn.textObject = (TMP_Text)EditorGUILayout.ObjectField("Text Object", _btn.textObject, typeof(TMP_Text), true);

        SerializedProperty textColors = serializedObject.FindProperty("textColors");
        EditorGUILayout.PropertyField(textColors, true);

        serializedObject.ApplyModifiedProperties();
    }
}
}

