using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader {

    public enum ControlType
    {
        HORIZONTAL, VERTICAL, A_BTN, B_BTN, X_BTN, Y_BTN
    }

    public enum Controller
    {
        ONE, TWO, THREE, FOUR
    }

    public static float getInput(Controller controller, ControlType controltype)
    {
        switch(controltype) {
            case ControlType.HORIZONTAL: {
                    float input = Input.GetAxis("Horizontal_" + controller.ToString());
                    if (input > 0.25)
                        return 1;
                    else if (input < -0.25)
                        return -1;
                    return 0;
                }
            case ControlType.VERTICAL:
                {
                    float input = Input.GetAxis("Vertical_" + controller.ToString());
                    if (input > 0.6)
                        return 1;
                    else if (input < -0.6)
                        return -1;
                    return 0;
                }
            case ControlType.A_BTN:
                {
                    return Input.GetAxis("A_BTN_" + controller.ToString());
                }
            case ControlType.B_BTN:
                {
                    return Input.GetAxis("B_BTN_" + controller.ToString());
                }
            case ControlType.X_BTN:
                {
                    return Input.GetAxis("X_BTN_" + controller.ToString());
                }
            case ControlType.Y_BTN:
                {
                    return Input.GetAxis("Y_BTN_" + controller.ToString());
                }
        }
        return 0;
    }

}
