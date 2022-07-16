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
    void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentInput = InputStates.Right;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentInput = InputStates.Left;
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            currentInput = InputStates.Enter;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentInput = InputStates.Back;
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentInput = InputStates.Exit;
        }

        else
        {
            currentInput = InputStates.Idle;
        }
    }


}
