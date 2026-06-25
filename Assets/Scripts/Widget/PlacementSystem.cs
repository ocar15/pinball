using UnityEngine;
using UnityEngine.InputSystem;
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
    private Vector3 pointerPosition;

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
        Debug.Log(mousePosition);
        Vector3 pointerPosition = GetPointerPosition(mousePosition);

        if(preview != null)
        {
            HandlePreview(pointerPosition);
        }
        else
        {
            if (debugOne.WasPerformedThisFrame())
            {
                preview = CreatePreview(widgetData, pointerPosition);
            }
        }
    }

    private void HandlePreview(Vector3 pointerPosition)
    {
        preview.transform.localPosition = pointerPosition;
        if (CanPlace(preview, table.grid))
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

    private bool CanPlace(WidgetPreview preview, WidgetGrid grid)
    {
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

    private Vector3 GetPointerPosition(Vector3 mousePosition)
    {
        WidgetGrid grid = table.grid;
        // If mouse is over grid, snap to it
        if(mousePosition.x >= grid.transform.localPosition.x &&
            mousePosition.x <= grid.transform.localPosition.x + grid.GetWidth() &&
            mousePosition.z >= grid.transform.localPosition.z &&
            mousePosition.z <= grid.transform.localPosition.z + grid.GetHeight())
        {
            float pointerX = mousePosition.x - (mousePosition.x % WidgetGridCell.Size);
            float pointerZ = mousePosition.z - (mousePosition.z % WidgetGridCell.Size);
            return new(pointerX, 0, pointerZ);
        }
        
        // Otherwise, set to mouse world position
        return mousePosition;
    }

    private WidgetPreview CreatePreview(WidgetData data, Vector3 position)
    {
        WidgetPreview widgetPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        widgetPreview.Setup(data, table.gameObject);
        return widgetPreview;
    }
}
