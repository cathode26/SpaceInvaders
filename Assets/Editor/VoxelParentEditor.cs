using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelParent))]
public class VoxelParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoxelParent voxelParent = (VoxelParent)target;
        if (GUILayout.Button("Center Parent"))
        {
            voxelParent.CenterParent();
        }
    }
}
