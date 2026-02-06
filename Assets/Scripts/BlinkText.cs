using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 0.5f;
    private TextMeshProUGUI text;
    private bool blinking = true;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        InvokeRepeating(nameof(ToggleText), 0, blinkSpeed);
    }

    void ToggleText()
    {
        if (blinking)
            text.enabled = !text.enabled;
    }

    public void StopBlinking()
    {
        blinking = false;
        CancelInvoke();
        text.enabled = false;
    }
}
