using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private DroneMother droneMother;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    internal Vector2 moveDirection;
    internal Vector2 aimDirection;
    internal float switchLeft;
    internal float switchRight;
    internal float actionA;
    internal float actionB;

    void Awake()
    {
        droneMother = GameObject.FindGameObjectWithTag("DroneMother").GetComponent<DroneMother>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.SwitchLeft.performed += SwitchLeft;
        playerInputActions.Player.SwitchRight.performed += SwitchRight;
    }

    private void OnEnable() 
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        moveDirection = playerInputActions.Player.Move.ReadValue<Vector2>();
        actionA = playerInputActions.Player.ActionA.ReadValue<float>();
        actionB = playerInputActions.Player.ActionB.ReadValue<float>();

        GetAimDirection();
    }

    void GetAimDirection()
    {
        if (playerInput.currentControlScheme.Equals("Gamepad"))
        {
            aimDirection = playerInputActions.Player.GamepadAim.ReadValue<Vector2>();
        }
        else if (playerInput.currentControlScheme.Equals("KBM"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(playerInputActions.Player.MouseAim.ReadValue<Vector2>());
            aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        }
    }

    void SwitchLeft(InputAction.CallbackContext context)
    {
        droneMother.SwapDrone(-1);
    }
    void SwitchRight(InputAction.CallbackContext context)
    {
        droneMother.SwapDrone(1);
    }
}
