using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
// Main program that contains basic gameplay loop. 
{
    // State
    public InputStates currentInput = InputStates.Idle; // enumeration index

    InputControl controls = new InputControl();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentInput = controls.currentInput;
    }
}

