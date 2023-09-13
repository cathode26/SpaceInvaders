using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FitColliderToChildren : MonoBehaviour
{
    [ContextMenu("Resize Collider to Fit Children")]
    public void ResizeColliderToFit()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // Initialize bounds to an invalid value at first
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool boundsStarted = false;

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
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

        boxCollider.center = bounds.center - transform.position;
        boxCollider.size = bounds.size;
    }
}
