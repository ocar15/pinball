using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject world;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private TableData tableData;
    [SerializeField] private Table tablePrefab;
    private Table table;

    void Start()
    {
        NewGame(tablePrefab);
    }

    private void NewGame(Table tablePrefab)
    {
        // Setup table
        table = Instantiate(tablePrefab, transform.position, Quaternion.identity, transform);
        table.Setup(tableData, world);

        // Setup placement system
        placementSystem.Setup(table);
    }
}
