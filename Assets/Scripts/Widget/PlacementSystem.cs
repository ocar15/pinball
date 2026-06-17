using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class PlacementSystem : MonoBehaviour
{
    public const float CellSize = 1f;
    [SerializeField] private WidgetData widgetData;
    [SerializeField] private WidgetPreview previewPrefab;
    [SerializeField] private Widget widgetPrefab;
    [SerializeField] private WidgetGrid grid;
    private WidgetPreview preview;

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
        List<Vector3> placePosition = preview.WidgetModel.GetAllBuildingPositions();
        bool canBuild = grid.CanPlace(placePosition);
        if (canBuild)
        {
            preview.transform.position = GetSnappedPosition(placePosition);
            preview.ChangeState(WidgetPreview.WidgetPreviewState.POSITIVE);
            if (Input.GetMouseButtonDown(0))
            {
                PlaceWidget(placePosition);
            }
        }
        else
        {
            preview.ChangeState(WidgetPreview.WidgetPreviewState.NEGATIVE);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.Rotate(90);
        }
    }

    private void PlaceWidget(List<Vector3> widgetPositions)
    {
        Widget widget = Instantiate(widgetPrefab, preview.transform.position, Quaternion.identity);
        widget.Setup(preview.Data, preview.WidgetModel.Rotation);
        grid.SetWidget(widget, widgetPositions);
        Destroy(preview.gameObject);
        preview = null;
    }

    private Vector3 GetSnappedPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
        float centerX = (xs.Min() + xs.Max()) / 2f + CellSize / 2f;
        float centerZ = (zs.Min() + zs.Max()) / 2f + CellSize / 2f;
        return new(centerX, 0, centerZ);
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
