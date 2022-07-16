using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public string controlID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        controlID = ProcessInput();
        ProcessSelection(controlID);
    }

    // TODO return inputState enumeration element found in GameMaster
    string ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return "right";
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return "left";
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            return "return";
        }

        else
        {
            return "";
        }
    }

    void ProcessSelection(string controlID)
    {
        if (controlID == "right")
        {
            Globals.currentInput
            Globals globals = new Globals();
        }

        if (controlID == "left")
        {
            Debug.Log("left");
        }

        if (controlID == "return")
        {
            Debug.Log("return");
        }
    }
}
