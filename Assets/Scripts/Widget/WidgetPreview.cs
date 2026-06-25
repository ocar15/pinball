using UnityEngine;
using System.Collections.Generic;

public class WidgetPreview : MonoBehaviour
{
    public enum WidgetPreviewState{ POSITIVE, NEGATIVE}

    [SerializeField] private Material positiveMaterial;
    [SerializeField] private Material negativeMaterial;
    public WidgetPreviewState State{get; private set;} = WidgetPreviewState.NEGATIVE;
    public WidgetData Data{get; private set;}
    public WidgetModel WidgetModel{get; private set;}
    private List<Renderer> renderers = new();
    private List<Collider> colliders = new();
    public int RotateStep{get; private set;}

    public void Setup(WidgetData data, GameObject parent)
    {
        Data = data;
        RotateStep = data.RotateStep;
        transform.parent = parent.transform;
        Vector3 modelPosition = new(transform.position.x + (data.Width * WidgetGridCell.Size / 2),
                                    transform.position.y,
                                    transform.position.z + (data.Height * WidgetGridCell.Size / 2));
        WidgetModel = Instantiate(data.Model, modelPosition, Quaternion.identity, transform);
        renderers.AddRange(WidgetModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(WidgetModel.GetComponentsInChildren<Collider>());
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
        SetPreviewMaterial(State);
    }

    public void ChangeState(WidgetPreviewState newState)
    {
        if(newState == State) return;
        State = newState;
        SetPreviewMaterial(State);
    }

    public void Rotate(int rotationStep)
    {
        WidgetModel.Rotate(rotationStep);
    }

    private void SetPreviewMaterial(WidgetPreviewState newState)
    {
        Material previewMat = newState == WidgetPreviewState.POSITIVE ? positiveMaterial : negativeMaterial;
        foreach(var renderer in renderers)
        {
            Material[] mats = new Material[renderer.sharedMaterials.Length];
            for(int i = 0; i < mats.Length; i++)
            {
                mats[i] = previewMat;
            }
            renderer.materials = mats;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw boundary box
        Vector3 bottomLeft = transform.TransformPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = transform.TransformPoint(new Vector3( Data.Width * WidgetGridCell.Size, 0, 0));
        Vector3 topLeft = transform.TransformPoint(new Vector3(0, 0, Data.Height * WidgetGridCell.Size));
        Vector3 topRight = transform.TransformPoint(new Vector3(Data.Width * WidgetGridCell.Size, 0, Data.Height * WidgetGridCell.Size));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}