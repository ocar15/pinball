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
        debugText.text += $"state: {plunger.GetState()}\n";
        debugText.text += $"force: {plunger.GetSpeed()}\n";
        debugText.text += $"percentPulled: {plunger.GetPercentPulled()}\n";
    }
}
