using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputStates
{ // each automatically assigned a value on 0-(len-1)
    Idle, // no input 
    Left, // a / left arrow / left click
    Right, // d / right arrow / right click
    Enter, // w / up arrow / middle click
    Back, // s / down arrow / spacebar
    Exit, // esc / quit button
}

public class InputControl : MonoBehaviour
{
    public string controlID;

    public InputStates currentInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    // TODO return inputState enumeration element found in GameMaster
    public InputStates ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return InputStates.Right;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return InputStates.Left;
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            return InputStates.Enter;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
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
