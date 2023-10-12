using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchManager : MonoBehaviour
{
    private InputControl inputControl;

    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private Joystick joystick;

    Vector3 target_vector;
    Vector2 prev_pos;

    private Finger movementFinger;

    private void Awake()
    {
        inputControl = InputControl.Instance;

        joystick.rectTransform.sizeDelta = joystickSize;
        joystick.rectTransform.anchoredPosition = new Vector2(joystickSize.x / 2 + 10, joystickSize.y / 2 + 10);
    }
    private void OnEnable()
    {
        inputControl.OnFingerDown += HandleFingerDown;
        inputControl.OnFingerMove += HandleFingerMove;
        inputControl.OnFingerUp += HandleFingerUp;

        target_vector = Vector3.zero;
        prev_pos = Vector3.zero;
    }

    private void OnDisable()
    {
        inputControl.OnFingerDown -= HandleFingerDown;
        inputControl.OnFingerMove -= HandleFingerMove;
        inputControl.OnFingerUp -= HandleFingerUp;
    }

    void resetTouch()
    {
        target_vector = Vector3.zero;
    }

    public Vector3 getTargetVector()
    {
        return target_vector;
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
            startPosition.x = joystickSize.x / 2;
        if (startPosition.y < joystickSize.y / 2)
            startPosition.y = joystickSize.y / 2;
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
            startPosition.y = Screen.height - joystickSize.y / 2;
        return startPosition;
    }


    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.currentTouch.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            target_vector = Vector3.zero;
            Vector3 touch_pos = touchedFinger.currentTouch.screenPosition;

            //joystick.gameObject.SetActive(true);
            //joystick.rectTransform.sizeDelta = joystickSize;
            //joystick.rectTransform.position = ClampStartPosition(touch_pos);
            // //joystick.rectTransform.anchoredPosition = new Vector2(joystickSize.x / 2 + 10, joystickSize.y / 2 + 10);
            prev_pos = touch_pos;
        }

    }
    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 touch_pos = movedFinger.currentTouch.screenPosition;
            target_vector = touch_pos - prev_pos;
            prev_pos = touch_pos;

            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2;
            if (Vector2.Distance(touch_pos, joystick.rectTransform.position) > maxMovement)
            {
                knobPosition = (touch_pos - new Vector2(joystick.rectTransform.position.x, joystick.rectTransform.position.y)
                    ).normalized * maxMovement;
            }
            else knobPosition = touch_pos - new Vector2(joystick.rectTransform.position.x, joystick.rectTransform.position.y);

            joystick.knob.anchoredPosition = knobPosition;

            target_vector = knobPosition / maxMovement;
        }
    }

    private void HandleFingerUp(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            resetTouch();

            target_vector = Vector2.zero;
            joystick.knob.anchoredPosition = Vector2.zero;
            //joystick.gameObject.SetActive(false);
        }
    }
}
