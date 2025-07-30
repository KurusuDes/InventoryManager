using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : Graphic
{
    public List<Vector2> points = new List<Vector2>();
    public float thickness = 5f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Count < 2)
            return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            AddLine(vh, points[i], points[i + 1]);
        }
    }

    private void AddLine(VertexHelper vh, Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        Vector2 normal = new Vector2(-direction.y, direction.x) * (thickness / 2f);

        Vector2 v1 = start + normal;
        Vector2 v2 = start - normal;
        Vector2 v3 = end + normal;
        Vector2 v4 = end - normal;

        int index = vh.currentVertCount;

        vh.AddVert(v1, color, Vector2.zero);
        vh.AddVert(v2, color, Vector2.zero);
        vh.AddVert(v3, color, Vector2.zero);
        vh.AddVert(v4, color, Vector2.zero);

        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index + 2, index + 1, index + 3);
    }

    void Update()
    {
        SetVerticesDirty();
    }
}