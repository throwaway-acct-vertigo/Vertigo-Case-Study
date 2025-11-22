using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "RewardData")]
public class RewardData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public string ID;
    public RewardType RewardType;
    public int DefaultQuantity = 1;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ID))
            return;

        string currentPath = AssetDatabase.GetAssetPath(this);
        string fileName = System.IO.Path.GetFileNameWithoutExtension(currentPath);
        
        if (fileName != ID)
        {
            string directory = System.IO.Path.GetDirectoryName(currentPath);
            string newPath = System.IO.Path.Combine(directory, ID + ".asset");
            
            string error = AssetDatabase.RenameAsset(currentPath, ID);
            if (string.IsNullOrEmpty(error))
            {
                EditorUtility.SetDirty(this);
            }
            else
            {
                Debug.LogError($"Failed to rename asset: {error}");
            }
        }
    }
#endif
}