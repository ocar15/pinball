using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Table")]
public class GameData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public Table Table {get; private set;}
}