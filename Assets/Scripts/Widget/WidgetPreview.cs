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
        WidgetModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
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
}