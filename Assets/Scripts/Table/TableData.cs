using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Table")]
public class TableData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public WidgetGrid Grid {get; private set;}
    [field: SerializeField] public Vector3 Rotation {get; private set;}
}