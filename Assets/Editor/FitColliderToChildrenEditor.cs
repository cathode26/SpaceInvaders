using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform), true)]
public class FitColliderToChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Resize Collider to Fit Children"))
        {
            Transform targetTransform = (Transform)target;
            BoxCollider boxCollider = targetTransform.GetComponent<BoxCollider>();

            if (boxCollider)
            {
                ResizeColliderToFit(targetTransform, boxCollider);
            }
            else
            {
                Debug.LogWarning("No BoxCollider found on the selected object.");
            }
        }
    }

    private void ResizeColliderToFit(Transform targetTransform, BoxCollider boxCollider)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool boundsStarted = false;

        foreach (Renderer renderer in targetTransform.GetComponentsInChildren<Renderer>())
        {
            if (!boundsStarted)
            {
                bounds = renderer.bounds;
                boundsStarted = true;
            }
            else
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

        boxCollider.center = bounds.center - targetTransform.position;
        boxCollider.size = bounds.size;
    }
}