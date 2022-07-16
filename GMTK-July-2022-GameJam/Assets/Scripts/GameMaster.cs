using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
// Main program that contains basic gameplay loop. 
{
    public enum gameStates { // each automatically assigned a value on 0-(len-1)
        GameStart,
        MainMenu,
        Loading,
        pause,
        Player1Turn,
        Player2Turn,
        GameOver,
        settings
    }

    public enum inputStates { // each automatically assigned a value on 0-(len-1)
        Idle, // no input 
        Left, // a / left arrow / left click
        right, // d / right arrow / right click
        Enter, // w / up arrow / middle click
        Back, // s / down arrow / spacebar
        Exit, // esc / quit button
    }

    // Settings
    public static int numDice = 3; // Number of dice that are rolled at once

    // State
    public gameStates gameState; // enumeration index
    public inputStates currentInput; // enumeration index

    public int[] activeDie; // Die still available
    public int selectedDie; // selected die/move
    public int selectedDot; // index of coordinate in move

    GameControls controls;

    void Start()
    {
        gameState = gameStates.GameStart;
        currentInput = inputStates.Idle;
        activeDie = Enumerable.Repeat(1, numDice).ToArray();

        controls = new GameControls();
    }

    // Update is called once per frame
    void Update()
    {
        // currentInput = controls.controlID;
    }
}

