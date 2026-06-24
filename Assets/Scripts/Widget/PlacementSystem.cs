using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class PlacementSystem : MonoBehaviour
{
    public const float CellSize = .3f;
    [SerializeField] private WidgetData widgetData;
    [SerializeField] private WidgetPreview previewPrefab;
    [SerializeField] private Widget widgetPrefab;
    private WidgetGrid grid;
    private WidgetPreview preview;

    public void Setup(WidgetGrid grid)
    {
        this.grid = grid;
    }

    private void Update()
    {
        Vector3 mousePos = GetMouseWorldPosition();

        if(preview != null)
        {
            HandlePreview(mousePos);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                preview = CreatePreview(widgetData, mousePos);
            }
        }
    }

    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        bool canBuild = grid.CanPlace(preview);
        // if (canBuild)
        // {
        //     preview.transform.position = GetSnappedPosition(placePosition);
        //     preview.ChangeState(WidgetPreview.WidgetPreviewState.POSITIVE);
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         PlaceWidget(placePosition);
        //     }
        // }
        // else
        // {
        //     preview.ChangeState(WidgetPreview.WidgetPreviewState.NEGATIVE);
        // }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     preview.Rotate(90);
        // }
    }

    private void PlaceWidget(List<Vector3> widgetPositions)
    {
        Widget widget = Instantiate(widgetPrefab, preview.transform.position, Quaternion.identity);
        widget.Setup(preview.Data, preview.WidgetModel.Rotation);
        grid.SetWidget(widget, widgetPositions);
        Destroy(preview.gameObject);
        preview = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);
        if(groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private WidgetPreview CreatePreview(WidgetData data, Vector3 position)
    {
        WidgetPreview widgetPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        widgetPreview.Setup(data);
        return widgetPreview;
    }
}
