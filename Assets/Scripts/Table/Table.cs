using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Table : MonoBehaviour
{
    public WidgetGrid grid {get; private set;}

    public void Setup(TableData data, GameObject parent)
    {
        grid = new WidgetGrid(data.Width, data.Height);
        GetComponent<MeshFilter>().mesh = GenerateMesh(grid);
        transform.parent = parent.transform;
        gameObject.transform.Rotate(data.Rotation);
    }

    private Mesh GenerateMesh(WidgetGrid grid)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new []
        {
            // Top plane
            new Vector3(0, 0, 0),
            new Vector3(0, 0, grid.GetHeight() * WidgetGridCell.Size),
            new Vector3(grid.GetWidth() * WidgetGridCell.Size, 0, grid.GetHeight() * WidgetGridCell.Size),
            new Vector3(grid.GetWidth() * WidgetGridCell.Size, 0, 0),

            // Bottom plane
            new Vector3(0, -5, 0),
            new Vector3(0, -5, grid.GetHeight() * WidgetGridCell.Size),
            new Vector3(grid.GetWidth() * WidgetGridCell.Size, -5, grid.GetHeight() * WidgetGridCell.Size),
            new Vector3(grid.GetWidth() * WidgetGridCell.Size, -5, 0),
        };
        int[] triangles = new []
        {
            // Top plane
            0, 1, 2,
            2, 3, 0,

            // // Bottom plane
            // 4, 5, 6,
            // 5, 7, 4,
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    public float GetWidth()
    {
        return grid.GetWidth() * WidgetGridCell.Size;
    }

    public float GetHeight()
    {
        return grid.GetHeight() * WidgetGridCell.Size;
    }

    public void OnDrawGizmos()
    {
        DrawGridLines();
    }

    private void DrawGridLines()
    {
        Gizmos.color = Color.yellow;
        if(WidgetGridCell.Size <= 0 || grid.GetWidth() <= 0 || grid.GetHeight() <= 0) return;
        Vector3 origin = transform.localPosition;

        for(int y = 0; y <= grid.GetHeight(); y++)
        {
            Vector3 start = origin + new Vector3(0, 0, y * WidgetGridCell.Size);
            Vector3 end = origin + new Vector3(grid.GetWidth() * WidgetGridCell.Size, 0, y * WidgetGridCell.Size);
            start = transform.TransformPoint(start);
            end = transform.TransformPoint(end);
            Gizmos.DrawLine(start, end);
        }
        for(int x = 0; x <= grid.GetWidth(); x++)
        {
            Vector3 start = origin + new Vector3(x * WidgetGridCell.Size, 0, 0);
            Vector3 end = origin + new Vector3(x * WidgetGridCell.Size, 0, grid.GetHeight() * WidgetGridCell.Size);
            start = transform.TransformPoint(start);
            end = transform.TransformPoint(end);
            Gizmos.DrawLine(start, end);
        }
    }
}
