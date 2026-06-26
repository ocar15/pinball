using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    InputAction interact;
    InputAction rotatePositive;
    InputAction rotateNegative;
    InputAction debugOne;

    [SerializeField] private WidgetData widgetData;
    [SerializeField] private WidgetPreview previewPrefab;
    [SerializeField] private Widget widgetPrefab;

    private Table table;
    private WidgetPreview preview;

    public void Setup(Table table)
    {
        this.table = table;
    }

    private void Start()
    {
        interact = InputSystem.actions.FindAction("Interact");
        rotatePositive = InputSystem.actions.FindAction("RotatePositive");
        rotateNegative = InputSystem.actions.FindAction("RotateNegative");
        debugOne = InputSystem.actions.FindAction("DebugOne");
    }

    private void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector2Int gridPosition = GetGridPosition(mousePosition);
        Vector3 pointerPosition = GetPointerPosition(mousePosition, gridPosition);

        if(preview != null)
        {
            HandlePreview(pointerPosition, gridPosition);
        }
        else
        {
            if (debugOne.WasPerformedThisFrame())
            {
                preview = CreatePreview(widgetData, pointerPosition);
            }
        }
    }

    private void HandlePreview(Vector3 pointerPosition, Vector2Int gridPosition)
    {
        preview.transform.localPosition = pointerPosition;
        if (CanPlace(preview, gridPosition))
        {
            preview.ChangeState(WidgetPreview.WidgetPreviewState.POSITIVE);
            if (interact.WasPerformedThisFrame())
            {
                PlaceWidget(pointerPosition);
            }
        }
        else
        {
            preview.ChangeState(WidgetPreview.WidgetPreviewState.NEGATIVE);
        }

        // Rotate preview
        if (rotatePositive.WasPerformedThisFrame()) preview.Rotate(preview.RotateStep);
        else if (rotateNegative.WasPerformedThisFrame()) preview.Rotate(preview.RotateStep * -1);
    }

    private bool CanPlace(WidgetPreview preview, Vector2Int gridPosition)
    {
        if(gridPosition.x < 0 || gridPosition.x >= table.grid.GetWidth()
        || gridPosition.y < 0 || gridPosition.y >= table.grid.GetHeight()) return false;

        // Using grid position, determine if cells in range are full or not
        WidgetGridCell[,] grid = table.grid.GetGrid();
        for(int i = gridPosition.x; i < gridPosition.x + preview.Data.Width; i++)
        {
            for(int j = gridPosition.y; j < gridPosition.y + preview.Data.Height; j++)
            {
                if(!grid[i,j].IsEmpty()) return false;
            }
        }
        return true;
    }

    private void PlaceWidget(Vector3 widgetPositions)
    {
        // Widget widget = Instantiate(widgetPrefab, preview.transform.position, Quaternion.identity);
        // widget.Setup(preview.Data, preview.WidgetModel.Rotation);
        // grid.SetWidget(widget, widgetPositions);
        // Destroy(preview.gameObject);
        // preview = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new(table.transform.up, Vector3.zero);
        if(groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private Vector3 GetPointerPosition(Vector3 mousePosition, Vector2 gridPosition)
    {
        if(gridPosition.x >= 0 && gridPosition.y >= 0)
        {
            float x = gridPosition.x * WidgetGridCell.Size;
            float z = gridPosition.y * WidgetGridCell.Size;

            return new(x, 0, z);
        }
        
        // Otherwise, set to mouse world position
        // Debug.Log(mousePosition);
        return mousePosition;
    }

    private Vector2Int GetGridPosition(Vector3 mousePosition)
    {
        if(mousePosition.x >= table.transform.localPosition.x &&
            mousePosition.x <= table.transform.localPosition.x + table.GetWidth() &&
            mousePosition.z >= table.transform.localPosition.z &&
            mousePosition.z <= table.transform.localPosition.z + table.GetHeight())
        {
            int x = (int) Mathf.Floor(mousePosition.x / WidgetGridCell.Size);
            int y = (int) Mathf.Floor(mousePosition.z / WidgetGridCell.Size);

            return new(x, y);
        }
        return new(-1, -1);
    }

    private WidgetPreview CreatePreview(WidgetData data, Vector3 position)
    {
        WidgetPreview widgetPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        widgetPreview.Setup(data, table.gameObject);
        return widgetPreview;
    }
}
