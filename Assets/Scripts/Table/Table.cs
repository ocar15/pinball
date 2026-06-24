using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Table : MonoBehaviour
{
    private TableData data;
    
    public WidgetGrid grid => data.Grid;
    public Vector3 rotation => data.Rotation;

    public void Setup(TableData data)
    {
        this.data = data;
        grid.transform.position = transform.position;
        GetComponent<MeshFilter>().mesh = GenerateMesh(grid);
        gameObject.transform.Rotate(rotation);
    }

    private Mesh GenerateMesh(WidgetGrid grid)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new []
        {
            // Top plane
            new Vector3(0, 0, 0),
            new Vector3(0, 0, grid.GetHeight()),
            new Vector3(grid.GetWidth(), 0, grid.GetHeight()),
            new Vector3(grid.GetWidth(), 0, 0),

            // Bottom plane
            new Vector3(0, -5, 0),
            new Vector3(0, -5, grid.GetHeight()),
            new Vector3(grid.GetWidth(), -5, grid.GetHeight()),
            new Vector3(grid.GetWidth(), -5, 0),
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
}
