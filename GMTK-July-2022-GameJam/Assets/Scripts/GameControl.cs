using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControl : MonoBehaviour
// Main program that contains basic gameplay loop. 
{
    // Settings
    public static int numDie = 3; // Number of dice that are rolled at once
    public static int gridSize = 4; // Number of tiles in each dimension

    // GameObject / Modules
    GameObject gameController;
    private InputControl controls;
    private GameUIControl ui;
    private GridControl grid;
    private Deck deck;

    // State
    public enum GameStates { // each automatically assigned a value on 0-(len-1)

        GameStart,
        DiceSelection,
        TileSelection,
        Moving,
        GameOver,
    }

    private GameStates currentState; // above enumeration 
    private int playerTurn; // 1 = Player 1 turn, 2 = Player 2 turn
    private InputStates currentInput;

    private int lastSelectedDie = 0;
    private int selectedDie = 0; // selected die/move
    private int lastSelectedTile = 0;
    private int selectedTile = 0; // index of coordinate in move

    private bool[] currentActiveDie;
    // private GameObject[] currentDice;
    private List<Vector2Int>[] currentMoves;

    // Player States
    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    // private GameObject[] player1Dice = new GameObject[3];
    private List<Vector2Int>[] player1Moves = new List<Vector2Int>[numDie];

    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    // private GameObject[] player2Dice = new GameObject[3];
    private List<Vector2Int>[] player2Moves = new List<Vector2Int>[numDie];

    void Start()
    {
        gameController = GameObject.Find("GameController");
        controls = gameController.GetComponent<InputControl>();
        ui = gameController.GetComponent<GameUIControl>();
        grid = gameController.GetComponent<GridControl>();

        currentState = GameStates.GameStart;
        currentInput = InputStates.Idle;
        playerTurn = 1;

        ui.HandleGameLoad();
        grid.GenerateTiles();
        GenerateDie(1);
        GenerateDie(2);
    }

    // Update is called once per frame
    void Update()
    {
        currentInput = controls.ProcessInput();
        if (currentInput == InputStates.Exit) ExitGame();
        else if (currentState == GameStates.GameStart) GameStartTick(currentInput);
        else if (currentState == GameStates.DiceSelection) DiceSelectionTick(currentInput);
        else if (currentState == GameStates.TileSelection) TileSelectionTick(currentInput);
    }

    void GameStartTick(InputStates input) {
        if (input == InputStates.Enter) {
            ui.HandleGameStart();
            RunDiceSelection();
        }
    }

    void SetPlayerVariables(int playerTurn) {

        if (playerTurn == 1) {
            currentActiveDie = player1ActiveDie;
            // currentDice = player1Dice;
            currentMoves = player1Moves;
        }
        else {
            currentActiveDie = player2ActiveDie;
            // currentDice = player2Dice;
            currentMoves = player2Moves;
        }
    }

    void RunDiceSelection() {
        currentState = GameStates.DiceSelection;
        if (player1ActiveDie.Count<bool>() <= 0) GenerateDie(1);
        if (player2ActiveDie.Count<bool>() <= 0) GenerateDie(2);
    }

    void DiceSelectionTick(InputStates input) {
        lastSelectedDie = selectedDie;
        if (input == InputStates.Enter) {
            ui.HandleSelectDie(selectedDie);
            RunTileSelection();
        }
        else if (input == InputStates.Left) {
            selectedDie--;
            while (selectedDie < 0 || !currentActiveDie[selectedDie]) {
                selectedDie--;
                if (selectedDie < 0) selectedDie = numDie-1;
            }
            if (lastSelectedDie != selectedDie) ui.HandleHoverDie(lastSelectedDie, selectedDie);
        }
        else if (input == InputStates.Right) {
            selectedDie++;
            while (selectedDie >= numDie || !currentActiveDie[selectedDie]) {
                selectedDie++;
                if (selectedDie >= 0) selectedDie = 0;
            }
            if (lastSelectedDie != selectedDie) ui.HandleHoverDie(lastSelectedDie, selectedDie);
        }
    }

    void RunTileSelection() {
        currentState = GameStates.TileSelection;

    }

    void TileSelectionTick(InputStates input) {
        lastSelectedTile = selectedTile;
        if (input == InputStates.Enter) {
            ui.HandleSelectTile(selectedTile);
            ExecuteMove();
        }
        else if (input == InputStates.Left) {
            selectedTile--;
            while (selectedDie < 0 || !currentActiveDie[selectedDie]) {
                selectedDie--;
                if (selectedDie < 0) selectedDie = numDie-1;
            }
            if (lastSelectedDie != selectedDie) ui.HandleHoverDie(lastSelectedDie, selectedDie);
        }
        else if (input == InputStates.Right) {
            selectedDie++;
            while (selectedDie >= numDie || !currentActiveDie[selectedDie]) {
                selectedDie++;
                if (selectedDie >= 0) selectedDie = 0;
            }
            if (lastSelectedDie != selectedDie) ui.HandleHoverDie(lastSelectedDie, selectedDie);
        }
    }

    void ExecuteMove() {
        currentState = GameStates.Moving;
        return;
    }

    void ExitGame() {
        Application.Quit();
    }

    void GenerateDie(int playerTurn) {
        SetPlayerVariables(playerTurn);

        List<Vector2Int> moveset;
        Transform diceTransform;
        GameObject dice;
        for (int i = 0; i < numDie; i++) {
            currentActiveDie[i] = true;
            (moveset, diceTransform) = GetDiceInfo();
        }
        return;
    }

    (List<Vector2Int>, Transform) GetDiceInfo() {
        GameObject dice = deck.RollTheDie();
        Transform diceTransform = dice.GetComponent<Transform>();
        List<Vector2Int> moveset = dice.GetComponent<ValidMoves>().GetMoves();
        return (moveset, diceTransform);
    }
}



    // After dotselection is run and the dot is selected:

    /*
    // First, we need a "bool HasValidMove(List<Vector2Int> listOfMovesToCheck)" function that takes in a list of moves and returns if at least one is valid
    // Run this once after each player's turn.
    void CheckAndReroll(){

        if (it was player 1's turn) {
            
            // Set accumulator
            i = 0;

            // For each item of player1ActiveDie...
            foreach(bool active in player1ActiveDie) {

                // If current bool is true AND at least one valid move exists in the corresponding gameObject, break the function. We found at least one move the player can make.
                if (active && HasValidMove(player1Moves[i].GetComponent<ValidMoves>().GetMoves())) {
                    return
                }
                
                // Increment the accumulator
                i++;
            }

            // If we made it through all the loops, then there couldn't have been a valid move. So reset the player's hand.
            player1Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
        }

        // Ditto for player 2
        if (it was player 2's turn) {
            i = 0;
            foreach(bool active in player1ActiveDie) {
                if (active && HasValidMove(player2Moves[i].GetComponent<ValidMoves>().GetMoves())) {
                    return
                }
                
                i++;
            }

            player2Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
        }
    }
    */
