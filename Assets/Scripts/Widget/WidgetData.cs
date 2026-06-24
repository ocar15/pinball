using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Widget")]
public class WidgetData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public string Description {get; private set;}
    [field: SerializeField] public int Cost {get; private set;}
    [field: SerializeField] public WidgetModel Model {get; private set;}
    [field: SerializeField] public int Width {get; private set;}
    [field: SerializeField] public int Height {get; private set;}
}
