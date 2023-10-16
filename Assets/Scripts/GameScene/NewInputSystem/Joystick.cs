using UnityEngine;

public class Joystick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rectTransform;
    public RectTransform knob;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void InitializeJoystick()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
