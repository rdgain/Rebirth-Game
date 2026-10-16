using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Centralizes gameplay controls using the Input System while preserving the legacy key bindings.</summary>
public static class GameplayInput
{
    private static InputAction moveLeft;
    private static InputAction moveRight;
    private static InputAction moveUp;
    private static InputAction moveDown;
    private static InputAction moveGamepad;
    private static InputAction action;
    private static InputAction tether;

    private static int actionReleasedFrame = -1;
    private static int tetherReleasedFrame = -1;

    /// <summary>Returns digital keyboard movement, falling back to the gamepad left stick per axis.</summary>
    public static Vector2 Movement
    {
        get
        {
            EnsureActions();

            Vector2 gamepad = moveGamepad.ReadValue<Vector2>();
            float horizontal = (moveRight.IsPressed() ? 1f : 0f) - (moveLeft.IsPressed() ? 1f : 0f);
            float vertical = (moveUp.IsPressed() ? 1f : 0f) - (moveDown.IsPressed() ? 1f : 0f);

            if (horizontal == 0f)
                horizontal = gamepad.x;
            if (vertical == 0f)
                vertical = gamepad.y;

            return new Vector2(horizontal, vertical);
        }
    }

    /// <summary>True during the frame in which the Action control is released.</summary>
    public static bool ActionReleasedThisFrame
    {
        get
        {
            EnsureActions();
            return actionReleasedFrame == Time.frameCount;
        }
    }

    /// <summary>True during the frame in which the Tether control is released.</summary>
    public static bool TetherReleasedThisFrame
    {
        get
        {
            EnsureActions();
            return tetherReleasedFrame == Time.frameCount;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        DisposeActions();
        actionReleasedFrame = -1;
        tetherReleasedFrame = -1;
    }

    private static void EnsureActions()
    {
        if (moveLeft != null)
            return;

        moveLeft = CreateKeyboardAction("Move Left", "<Keyboard>/leftArrow", "<Keyboard>/a");
        moveRight = CreateKeyboardAction("Move Right", "<Keyboard>/rightArrow", "<Keyboard>/d");
        moveUp = CreateKeyboardAction("Move Up", "<Keyboard>/upArrow", "<Keyboard>/w");
        moveDown = CreateKeyboardAction("Move Down", "<Keyboard>/downArrow", "<Keyboard>/s");

        moveGamepad = new InputAction("Move Gamepad", InputActionType.Value, expectedControlType: "Vector2");
        moveGamepad.AddBinding("<Gamepad>/leftStick");

        action = CreateReleaseAction("Action", "<Keyboard>/e", OnActionReleased);
        tether = CreateReleaseAction("Tether", "<Keyboard>/t", OnTetherReleased);

        moveLeft.Enable();
        moveRight.Enable();
        moveUp.Enable();
        moveDown.Enable();
        moveGamepad.Enable();
        action.Enable();
        tether.Enable();
    }

    private static InputAction CreateKeyboardAction(string name, string firstBinding, string secondBinding)
    {
        InputAction inputAction = new InputAction(name, InputActionType.Button);
        inputAction.AddBinding(firstBinding);
        inputAction.AddBinding(secondBinding);
        return inputAction;
    }

    private static InputAction CreateReleaseAction(string name, string binding, System.Action<InputAction.CallbackContext> onReleased)
    {
        InputAction inputAction = new InputAction(name, InputActionType.Button);
        inputAction.AddBinding(binding);
        inputAction.canceled += onReleased;
        return inputAction;
    }

    private static void OnActionReleased(InputAction.CallbackContext context)
    {
        actionReleasedFrame = Time.frameCount;
    }

    private static void OnTetherReleased(InputAction.CallbackContext context)
    {
        tetherReleasedFrame = Time.frameCount;
    }

    private static void DisposeActions()
    {
        moveLeft?.Dispose();
        moveRight?.Dispose();
        moveUp?.Dispose();
        moveDown?.Dispose();
        moveGamepad?.Dispose();
        action?.Dispose();
        tether?.Dispose();

        moveLeft = null;
        moveRight = null;
        moveUp = null;
        moveDown = null;
        moveGamepad = null;
        action = null;
        tether = null;
    }
}
