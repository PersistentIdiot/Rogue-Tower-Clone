using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public PathPoint NextPoint = null;
    public bool IsStart = false;
    public bool IsEnd = false;

    public static float yOffset = 0.5f;
}