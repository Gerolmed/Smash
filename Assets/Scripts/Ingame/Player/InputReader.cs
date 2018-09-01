using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputReader {

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public enum ControlType
    {
        HORIZONTAL, VERTICAL, A_BTN, B_BTN, X_BTN, Y_BTN
    }

    public enum Controller
    {
        ONE, TWO, THREE, FOUR
    }

    void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    public static float getInput(Controller controller, ControlType controltype)
    {
        PlayerIndex testPlayerIndex = (PlayerIndex)0;
        GamePadState state = GamePad.GetState(testPlayerIndex);

        bool noConnect = false;

        switch (controller)
        {
            case Controller.ONE: {
                    if (ControllerHub.Instance.controllerCount() < 1)
                    {
                        noConnect = true;
                        break;
                    }
                    state = GamePad.GetState(ControllerHub.Instance.GetControls()[0]);
                    break;
                }
            case Controller.TWO:
                {
                    if (ControllerHub.Instance.controllerCount() < 2)
                    {
                        noConnect = true;
                        break;
                    }
                    state = GamePad.GetState(ControllerHub.Instance.GetControls()[1]);
                    break;
                }
            case Controller.THREE:
                {
                    if (ControllerHub.Instance.controllerCount() < 3)
                    {
                        noConnect = true;
                        break;
                    }
                    state = GamePad.GetState(ControllerHub.Instance.GetControls()[2]);
                    break;
                }
            case Controller.FOUR:
                {
                    if (ControllerHub.Instance.controllerCount() < 4)
                    {
                        noConnect = true;
                        break;
                    }
                    state = GamePad.GetState(ControllerHub.Instance.GetControls()[3]);
                    break;
                }
        }

        if (noConnect)
            return 0;

        switch(controltype)
        {
            case ControlType.HORIZONTAL:
                {
                    float input = state.ThumbSticks.Left.X;
                    if (input > 0.25)
                        return 1;
                    else if (input < -0.25)
                        return -1;
                    return 0;
                }
            case ControlType.VERTICAL:
                {
                    float input = state.ThumbSticks.Left.Y;
                    if (input > 0.25)
                        return 1;
                    else if (input < -0.25)
                        return -1;
                    return 0;
                }
            case ControlType.A_BTN:
                {
                    return (state.Buttons.A == ButtonState.Pressed) ? 1 : 0;
                }
            case ControlType.B_BTN:
                {
                    return (state.Buttons.B == ButtonState.Pressed) ? 1 : 0;
                }
            case ControlType.X_BTN:
                {
                    return (state.Buttons.X == ButtonState.Pressed) ? 1 : 0;
                }
            case ControlType.Y_BTN:
                {
                    return (state.Buttons.Y == ButtonState.Pressed) ? 1 : 0;
                }
            default: return 0;
                
        }
    }
}
