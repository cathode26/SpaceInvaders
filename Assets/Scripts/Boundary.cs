using UnityEngine;

public class Boundary : MonoBehaviour
{
    public enum BoundaryType
    {
        TopBoundary,
        BottomBoundary,
        LeftBoundary,
        RightBoundary,
    }

    public BoundaryType boundaryType;
}
