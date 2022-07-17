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

    // The Deck
    [SerializeField] private Deck theDeck;

    // Settings
    private static int numDie = 3; // Number of dice that are rolled at once

    // State
    private InputControl controls = new InputControl();
    private GridControl grid = new GridControl();

    private int playerTurn = 1; // 1 = Player 1 turn, 2 = Player 2 turn
    private GameStates currentState = GameStates.GameStart; // enumeration index
    private InputStates currentInput = InputStates.Idle; // enumeration index

    // private int selectedDie = 0; // selected die/move
    // private int selectedDot = 0; // index of coordinate in move

    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private List<Vector2Int>[] player1Moves = new List<Vector2Int>[numDie];
    private GameObject[] player1Dice = new GameObject[3];


    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private List<Vector2Int>[] player2Moves = new List<Vector2Int>[numDie];
    private GameObject[] player2Dice = new GameObject[3];

    void Start()
    {
        grid.GenerateTiles();
        player1Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
        player2Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
    }

    // Update is called once per frame
    void Update()
    {
        currentInput = controls.ProcessInput();
        if (currentInput == InputStates.Exit) ExitGame();
        else if (currentState == GameStates.GameStart) GameStartTick(currentInput);
        else if (currentState == GameStates.DiceSelection) DiceSelectionTick(currentInput, playerTurn);
        else if (currentState == GameStates.DotSelection) DotSelectionTick(currentInput, playerTurn);
    }

    void GameStartTick(InputStates input) {
        if (input == InputStates.Enter) RunDiceSelection();
    }

    void RunDiceSelection() {
        currentState = GameStates.DiceSelection;
        
        if (playerTurn == 1) {
            GameObject[] die = player1Dice;
            bool[] activeDie = player1ActiveDie;
        }
        else {
            GameObject[] die = player2Dice;
            bool[] activeDie = player2ActiveDie;
        }
        for (int i = 0; i < numDie; i++) {
            
        }

    }

    void DiceSelectionTick(InputStates input, int playerTurn) {
        if (input == InputStates.Enter) RunDotSelection();
    }

    void RunDotSelection() {
        currentState = GameStates.DotSelection;
    }

    void DotSelectionTick(InputStates input, int playerturn) {
        if (input == InputStates.Enter) RunMove();
    }

    void RunMove() {
        currentState = GameStates.Moving;
        return;
    }

    void ExitGame() {
        Application.Quit();
    }
}

