using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControl : MonoBehaviour
// Main program that contains basic gameplay loop. 
{
    public enum GameStates { // each automatically assigned a value on 0-(len-1)
        GameStart,
        DiceSelection,
        DotSelection,
        Moving,
        GameOver,
    }

    // Settings
    private static int numDie = 3; // Number of dice that are rolled at once

    // State
    private InputControl controls = new InputControl();
    private GridControl grid = new GridControl();

    private int playerTurn = 1; // 1 = Player 1 turn, 2 = Player 2 turn
    private GameStates currentState = GameStates.GameStart; // enumeration index
    private InputStates currentInput = InputStates.Idle; // enumeration index

    private int selectedDie = 0; // selected die/move
    private int selectedDot = 0; // index of coordinate in move

    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private List<(int, int)>[] player1Moves = new List<(int, int)>[numDie];

    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private List<(int, int)>[] player2Moves = new List<(int, int)>[numDie];

    void Start()
    {
        grid.GenerateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        currentInput = controls.currentInput;
        if (currentInput == InputStates.Exit) ExitGame();
        else if (currentState == GameStates.GameStart) RunGameStart(currentInput);
        else if (currentState == GameStates.DiceSelection) RunDiceSelection(currentInput, playerTurn);
        else if (currentState == GameStates.DotSelection) RunDotSelection(currentInput, playerTurn);
    }

    void RunGameStart(InputStates input) {
        return;
    }

    void RunDiceSelection(InputStates input, int playerTurn) {
        return;
    }

    void RunDotSelection(InputStates input, int playerturn) {
        return;
    }

    void ExitGame() {
        return;
    }
}

