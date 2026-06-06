using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;
    private string data;

    [SerializeField] private Plunger plunger;
    [SerializeField] private FlipperController flipperController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "";

        // Plunger stats
        debugText.text += $"Plunger\n";
        debugText.text += $"ready: {plunger.IsReady()}\n";
        debugText.text += $"pulling: {plunger.IsPulling()}\n";
        debugText.text += $"primed: {plunger.IsPrimed()}\n";
        debugText.text += $"pullValueRaw: {plunger.GetPullValueRaw()}\n";
        debugText.text += $"pullValue: {plunger.GetPullValue()}\n";
        debugText.text += $"targetDistance: {plunger.GetTargetDistance()}\n";
        debugText.text += $"speed: {plunger.GetSpeed()}";
    }
}
