using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Player/InputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
{
    public event Action DashEvent;
    public event Action<bool> AttackEvent;
    public event Action<Vector2> MouseMoveEvent;
    public event Action OpenMenuEvent;
    public event Action<bool> MovementEvent;
    public event Action<int> SlotChangeEvent;
    public event Action InteractEvent;
        
    public Vector2 InputDirection { get; private set; }
    public Vector2 MousePos { get; private set; }
    
    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.UI.SetCallbacks(this);
        }
        _controls.Player.Enable();
        _controls.UI.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.UI.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputDirection = context.ReadValue<Vector2>().normalized;
        
        if (context.performed)
            MovementEvent?.Invoke(true);
        else if(context.canceled)
            MovementEvent?.Invoke(false);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            AttackEvent?.Invoke(true);
        else if (context.canceled)
            AttackEvent?.Invoke(false);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
            DashEvent?.Invoke();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        MousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        
        if (context.performed)
            MouseMoveEvent?.Invoke(MousePos);
    }

    public void OnEquit1(InputAction.CallbackContext context)
    {
        if (context.performed)
            SlotChangeEvent?.Invoke(0);
    }

    public void OnEquit2(InputAction.CallbackContext context)
    {
        if (context.performed)
            SlotChangeEvent?.Invoke(1);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            InteractEvent?.Invoke();
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
            OpenMenuEvent?.Invoke();
    }

    public void SetPlayerInput(bool isEnable)
    {
        if(isEnable)
            _controls.Player.Enable();
        else
            _controls.Player.Disable();
    }

    #region UIActions

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }
    

    #endregion
}
