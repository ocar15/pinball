using UnityEngine;
using System.Collections.Generic;

public class WidgetGrid
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private WidgetGridCell[,] grid;

    public WidgetGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new WidgetGridCell[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                grid[x, y] = new();
            }
        }
    }

    public WidgetGridCell[,] GetGrid()
    {
        return grid;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

}

public class WidgetGridCell
{
    public const float Size = .3f;
    private bool isEmpty;

    public WidgetGridCell()
    {
        this.isEmpty = false;
    }

    public void Place()
    {
        this.isEmpty = false;
    }

    public void Remove()
    {
        this.isEmpty = true;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }
}
