using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CustomEditor(typeof(Selectable), true), CanEditMultipleObjects]
public class SelectableNewEditor : SelectableEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public override VisualElement CreateInspectorGUI()
    {
        return base.CreateInspectorGUI();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
