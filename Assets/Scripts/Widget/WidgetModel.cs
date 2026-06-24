using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WidgetModel : MonoBehaviour
{
    [SerializeField] private Transform wrapper;
    public float Rotation => wrapper.transform.eulerAngles.y;

    private void Awake()
    {
    }

    public void Rotate(float rotationStep)
    {
        wrapper.Rotate(new(0, rotationStep, 0));
    }
}
