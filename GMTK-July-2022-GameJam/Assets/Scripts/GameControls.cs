using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public string controlID;

    public enum inputStates
    { // each automatically assigned a value on 0-(len-1)
        Idle, // no input 
        Left, // a / left arrow / left click
        Right, // d / right arrow / right click
        Enter, // w / up arrow / middle click
        Back, // s / down arrow / spacebar
        Exit, // esc / quit button
    }

    public inputStates inputEnum;

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
            inputEnum = inputStates.Right;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputEnum = inputStates.Left;
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            inputEnum = inputStates.Enter;
        }

        else
        {
            inputEnum = inputStates.Idle;
        }
    }


}
