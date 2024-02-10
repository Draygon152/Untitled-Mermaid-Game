using UnityEngine;

public static class RectTransformExtensions
{
    public static bool Overlaps(this RectTransform self, RectTransform other)
    {
        Vector3[] corners = new Vector3[4];
        self.GetWorldCorners(corners);
        Rect rect1 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        other.GetWorldCorners(corners);
        Rect rect2 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

        return rect1.Overlaps(rect2);
    }
}