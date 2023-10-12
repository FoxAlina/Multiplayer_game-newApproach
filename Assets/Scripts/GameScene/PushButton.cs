using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PushButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent ifButtonDown;
    public UnityEvent ifButtonUp;

    private bool isDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }

    void Update()
    {
        if (!isDown)
        {
            if (ifButtonUp != null)
                ifButtonUp.Invoke();
        }
        else
        {
            if (ifButtonDown != null)
                ifButtonDown.Invoke();
        }

    }
}
