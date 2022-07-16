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

public class GameControls : MonoBehaviour
{
    public string controlID;

    public InputStates inputEnum;

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
            inputEnum = InputStates.Right;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputEnum = InputStates.Left;
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            inputEnum = InputStates.Enter;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputEnum = InputStates.Back;
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            inputEnum = InputStates.Exit;
        }

        else
        {
            inputEnum = InputStates.Idle;
        }
    }


}
