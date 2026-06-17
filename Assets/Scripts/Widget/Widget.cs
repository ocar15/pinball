using UnityEngine;

public class Widget : MonoBehaviour
{
    private WidgetModel model;
    private WidgetData data;

    public string Name => data.Name;
    public string Description => data.Description;
    public int Cost => data.Cost;

    public void Setup(WidgetData data, float rotation)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);
    }
}
