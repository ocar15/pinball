using UnityEngine;
using System.Collections.Generic;

public class WidgetGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    public WidgetGridCell[,] grid;

    public void Start()
    {
        grid = new WidgetGridCell[width, height];
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

    public bool CanPlace(List<Vector3> allWidgetPositions)
    {
        foreach(var p in allWidgetPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            if(x < 0 || x >= width || y < 0 || y >= height) return false;
            if(!grid[x, y].IsEmpty()) return false;
        }
        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / PlacementSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).y / PlacementSystem.CellSize);
        return (x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(PlacementSystem.CellSize <= 0 || width <= 0 || height <= 0) return;
        Vector3 origin = transform.position;

        for(int y = 0; y <= height; y++)
        {
            Vector3 start = origin + new Vector3(0, .01f, y * PlacementSystem.CellSize);
            Vector3 end = origin + new Vector3(width * PlacementSystem.CellSize, .01f, y * PlacementSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
        for(int x = 0; x <= width; x++)
        {
            Vector3 start = origin + new Vector3(x * PlacementSystem.CellSize, .01f, 0);
            Vector3 end = origin + new Vector3(x * PlacementSystem.CellSize, .01f, height * PlacementSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
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
