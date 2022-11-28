using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputStates
{ 
    Idle, // no input 
    Left, // a / left arrow / left click
    Right, // d / right arrow / right click
    Enter, // w / up arrow / middle click
    Back, // s / down arrow / spacebar
    Exit, // esc / quit button
}

public class InputControl : MonoBehaviour
{

    public InputStates ProcessInput()
    {
        if ((Input.GetKeyDown(KeyCode.RightArrow)) || (Input.GetKeyDown(KeyCode.D)) || (Input.GetMouseButtonDown(1)))
        {
            return InputStates.Right;
        }

        else if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.A)) || (Input.GetMouseButtonDown(0)))
        {
            return InputStates.Left;
        }

        else if ((Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.W)) || (Input.GetMouseButtonDown(2)))
        {
            return InputStates.Enter;
        }

        else if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.Space)))
        {
            return InputStates.Back;
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            return InputStates.Exit;
        }

        else
        {
            return InputStates.Idle;
        }
    }


}
