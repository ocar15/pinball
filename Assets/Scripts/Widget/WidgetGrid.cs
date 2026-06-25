using UnityEngine;
using System.Collections.Generic;

public class WidgetGrid : MonoBehaviour
{
    [SerializeField] private int cellWidth;
    [SerializeField] private int cellHeight;
    public WidgetGridCell[,] grid;

    public void Start()
    {
        grid = new WidgetGridCell[cellWidth, cellHeight];
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new();
            }
        }
    }

    public void SetWidget(Widget widget, List<Vector3> allWidgetPositions)
    {
        foreach(var p in allWidgetPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].SetWidget(widget);
        }   
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / PlacementSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).y / PlacementSystem.CellSize);
        return (x, y);
    }

    private void OnDrawGizmos()
    {
        DrawGridLines();
    }

    private void DrawGridLines()
    {
        Gizmos.color = Color.yellow;
        if(PlacementSystem.CellSize <= 0 || cellWidth <= 0 || cellHeight <= 0) return;
        Vector3 origin = transform.localPosition;

        for(int y = 0; y <= cellHeight; y++)
        {
            Vector3 start = origin + new Vector3(0, 0, y * PlacementSystem.CellSize);
            Vector3 end = origin + new Vector3(cellWidth * PlacementSystem.CellSize, 0, y * PlacementSystem.CellSize);
            start = transform.TransformDirection(start);
            end = transform.TransformDirection(end);
            Gizmos.DrawLine(start, end);
        }
        for(int x = 0; x <= cellWidth; x++)
        {
            Vector3 start = origin + new Vector3(x * PlacementSystem.CellSize, 0, 0);
            Vector3 end = origin + new Vector3(x * PlacementSystem.CellSize, 0, cellHeight * PlacementSystem.CellSize);
            start = transform.TransformDirection(start);
            end = transform.TransformDirection(end);
            Gizmos.DrawLine(start, end);
        }
    }

    public float GetWidth()
    {
        return cellWidth * PlacementSystem.CellSize;
    }

    public float GetHeight()
    {
        return cellHeight * PlacementSystem.CellSize;
    }
}

public class WidgetGridCell
{
    private Widget widget;

    public void SetWidget(Widget widget)
    {
        this.widget = widget;
    }

    public bool IsEmpty()
    {
        return widget == null;
    }
}
