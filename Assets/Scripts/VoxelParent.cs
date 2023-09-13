using System.Collections.Generic;
using UnityEngine;

public class VoxelParent : MonoBehaviour
{
    [ContextMenu("Center Parent")]
    public void CenterParent()
    {
        Vector3 center = CalculateCenter();

        List<Transform> children = new List<Transform> ();
        foreach (Transform child in transform)
            children.Add (child);
        foreach (Transform child in children)
            child.SetParent(null);

        transform.position = center;

        foreach (Transform child in children)
            child.SetParent(transform);

    }

    private Vector3 CalculateCenter()
    {
        if (transform.childCount == 0) return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Transform child in transform)
        {
            sum += child.position;
        }
        return sum / transform.childCount;
    }
}
