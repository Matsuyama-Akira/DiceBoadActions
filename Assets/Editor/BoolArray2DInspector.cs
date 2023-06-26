using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(YourScriptContainingBoolArray2D))]
public class BoolArray2DInspector : Editor
{
    private SerializedProperty boolArray2D;

    private void OnEnable()
    {
        boolArray2D = serializedObject.FindProperty("bools");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int y = 0; y < boolArray2D.arraySize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < boolArray2D.GetArrayElementAtIndex(y).arraySize; x++)
            {
                SerializedProperty boolElement = boolArray2D.GetArrayElementAtIndex(y).GetArrayElementAtIndex(x);
                boolElement.boolValue = EditorGUILayout.Toggle(boolElement.boolValue);
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
