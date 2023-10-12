using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputControl : Singleton<InputControl>
{
    #region events
    public delegate void FingerDown(Finger obj);
    public event FingerDown OnFingerDown;
    public delegate void FingerMove(Finger obj);
    public event FingerMove OnFingerMove;
    public delegate void FingerUp(Finger obj);
    public event FingerUp OnFingerUp;
    #endregion

    private PlayerControls playerControls;

    private Camera mainCamera;

    private void Awake()
    {
        playerControls = new PlayerControls();
        
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();

        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += TouchOnFingerDown;
        ETouch.Touch.onFingerUp += TouchOnFingerUp;
        ETouch.Touch.onFingerMove += TouchOnFingerMove;

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();


        ETouch.Touch.onFingerDown -= TouchOnFingerDown;
        ETouch.Touch.onFingerUp -= TouchOnFingerUp;
        ETouch.Touch.onFingerMove -= TouchOnFingerMove;

        EnhancedTouchSupport.Disable();

        TouchSimulation.Disable();
    }

    private void TouchOnFingerMove(Finger obj)
    {
        if (OnFingerMove != null) OnFingerMove(obj);
    }

    private void TouchOnFingerUp(Finger obj)
    {
        if (OnFingerUp != null) OnFingerUp(obj);
    }

    private void TouchOnFingerDown(Finger obj)
    {
        if (OnFingerDown != null) OnFingerDown(obj);
    }
}
