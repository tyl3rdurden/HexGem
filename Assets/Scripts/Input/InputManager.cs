//#define DEBUGGING

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Manager/InputManager", fileName = "InputManager")]
public class InputManager : InitializableSO
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PauseManager pauseManager;
    
    public event Action<Vector2> OnPressEvent;
    public event Action<Vector2> OnDragEvent;
    public event Action OnReleaseEvent;
    public event Action OnBackEvent;

    private TouchInput touchControls;
    private bool isPress;

    private InputAction touchPosition;
    
    protected override void Init()
    {
        touchControls = new TouchInput();
        touchControls.Enable();

        touchControls.Touch.TouchPress.started += OnPress;
        touchControls.Touch.TouchPress.canceled += OnRelease;
        touchControls.Touch.TouchHold.started += OnDrag;
        touchControls.Touch.Back.started += OnBack;

        touchPosition = touchControls.Touch.TouchPosition;
    }

    protected override void DeInit()
    {
        OnPressEvent = null;
        OnDragEvent = null;
        OnReleaseEvent = null;
        OnBackEvent = null;

        if (touchControls == null) return;
     
        touchControls.Touch.TouchPress.started -= OnPress;
        touchControls.Touch.TouchPress.canceled -= OnRelease;
        touchControls.Touch.TouchHold.started -= OnDrag;
        touchControls.Touch.Back.started -= OnBack;
        
        touchControls.Disable();
    }

    private void OnPress(InputAction.CallbackContext callbackContext)
    {
        if (IsInputNotValidState()) 
            return;
        
        isPress = true;
        OnPressEvent?.Invoke(touchPosition.ReadValue<Vector2>());
        
#if DEBUGGING
        Debug.Log("Press");
#endif
    }

    private void OnRelease(InputAction.CallbackContext callbackContext)
    {
        if (IsInputNotValidState()) 
            return;
        
        isPress = false;
        OnReleaseEvent?.Invoke();
        
#if DEBUGGING
        Debug.Log("Release");
#endif
    }

    private void OnDrag(InputAction.CallbackContext callbackContext)
    {
        if (IsInputNotValidState()) 
            return;
        
        if (isPress == false) return;
        
        OnDragEvent?.Invoke(touchPosition.ReadValue<Vector2>());
        
#if DEBUGGING
        Debug.Log($"Dragging {touchPosition.ReadValue<Vector2>()}");
#endif
    }

    private void OnBack(InputAction.CallbackContext obj)
    {
        OnBackEvent?.Invoke();
        
#if DEBUGGING
        Debug.Log($"Back Button Pressed");
#endif
    }

    private bool IsInputNotValidState()
    {
        return (gameStateManager.currentStateType != GameStateType.InGame) || pauseManager.isGamePaused;
    }
}