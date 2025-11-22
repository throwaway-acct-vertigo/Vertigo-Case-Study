#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelConfigSO))]
[CanEditMultipleObjects]
public class WheelConfigSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Slices for All Selected"))
        {
            foreach (Object t in targets)
            {
                WheelConfigSO wheel = (WheelConfigSO)t;
                wheel.GenerateSlices();

                // Mark each ScriptableObject as changed
                EditorUtility.SetDirty(wheel);
            }

            // Ensure the editor refreshes
            AssetDatabase.SaveAssets();
        }
    }
}
#endif