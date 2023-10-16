using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class ExtendedTouchManager : MonoBehaviour
{
    private InputControl inputControl;

    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private Vector2 rotationAreaSize = new Vector2(200, 200);
    [SerializeField] private Joystick joystick;
    [SerializeField] private RectTransform rotationJoystickArea;

    Vector3 target_rotation_vector;
    Vector3 target_translation_vector;

    private Finger movementFinger;

    private void Awake()
    {
        inputControl = InputControl.Instance;

        joystick.InitializeJoystick();
        joystick.rectTransform.sizeDelta = joystickSize;
        joystick.rectTransform.anchoredPosition = new Vector2(joystickSize.x / 2 + 10, joystickSize.y / 2 + 10);

        rotationJoystickArea.sizeDelta = rotationAreaSize;
    }

    private void OnEnable()
    {
        inputControl.OnFingerDown += HandleFingerDown;
        inputControl.OnFingerMove += HandleFingerMove;
        inputControl.OnFingerUp += HandleFingerUp;

        resetTouch();
    }

    private void OnDisable()
    {
        inputControl.OnFingerDown -= HandleFingerDown;
        inputControl.OnFingerMove -= HandleFingerMove;
        inputControl.OnFingerUp -= HandleFingerUp;
    }

    void resetTouch()
    {
        target_rotation_vector = Vector3.zero;
        target_translation_vector = Vector3.zero;
    }

    public (Vector3, Vector3) getTargetVector()
    {
        return (target_rotation_vector, target_translation_vector);
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
            resetTouch();
            Vector3 touch_pos = touchedFinger.currentTouch.screenPosition;

            joystick.gameObject.SetActive(true);
            //joystick.rectTransform.sizeDelta = joystickSize;
            joystick.rectTransform.position = ClampStartPosition(touch_pos);
            // //joystick.rectTransform.anchoredPosition = new Vector2(joystickSize.x / 2 + 10, joystickSize.y / 2 + 10);
        }
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 touch_position = movedFinger.currentTouch.screenPosition;

            Vector2 rotationVector;
            float joystickAreaMaxMovement = joystickSize.x / 2;
            if (Vector2.Distance(touch_position, joystick.rectTransform.anchoredPosition) > joystickAreaMaxMovement)
            {
                rotationVector = (touch_position - new Vector2(joystick.rectTransform.anchoredPosition.x, joystick.rectTransform.anchoredPosition.y)
                    ).normalized * joystickAreaMaxMovement;
            }
            else { 
                rotationVector = touch_position - new Vector2(joystick.rectTransform.anchoredPosition.x, joystick.rectTransform.anchoredPosition.y);
            }

            joystick.knob.anchoredPosition = rotationVector;
            target_rotation_vector = rotationVector / joystickAreaMaxMovement;

            Vector2 movementVector;
            float rotationAreaMaxMovement = rotationAreaSize.x / 2;
            if (Vector2.Distance(touch_position, joystick.rectTransform.anchoredPosition) > rotationAreaMaxMovement)
            {
                movementVector = (touch_position - new Vector2(joystick.rectTransform.anchoredPosition.x, joystick.rectTransform.anchoredPosition.y)
                    ).normalized * rotationAreaMaxMovement;
                movementVector = rotationVector - movementVector;

                target_translation_vector = movementVector / (joystickAreaMaxMovement - rotationAreaMaxMovement);
            }
            else target_translation_vector = Vector3.zero;
        }
    }

    private void HandleFingerUp(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            resetTouch();
            joystick.knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
        }
    }
}
