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

    private bool[] currentActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private Transform[] currentDieTransform = new Transform[numDie];
    private List<(int, int)>[] currentMoves = new List<(int, int)>[numDie];

    // Player States
    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private Transform[] player1DieTransform = new Transform[numDie];
    private List<(int, int)>[] player1Moves = new List<(int, int)>[numDie];

    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private Transform[] player2DieTransform = new Transform[numDie];
    private List<(int, int)>[] player2Moves = new List<(int, int)>[numDie];

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
            currentDiceTransform = player1DieTransform;
            currentMoves = player1Moves;
        }
        else {
            currentActiveDie = player2ActiveDie;
            currentDiceTransform = player2DieTransform;
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

        List<(int, int)> moveset = new List<(int, int)>();
        List<(int, int)> moveCoordinates = new List<(int, int)>();
        Transform diceTransform;
        for (int i = 0; i < numDie; i++) {
            currentActiveDie[i] = true;

            moveset.Clear();
            moveCoordinates.Clear();
            while (moveCoordinates.Count < 1) {
                (moveset, diceTransform) = GetDiceInfo();
                moveCoordinates = grid.GetValidMoveCoordinates(playerTurn, moveset);
            }
            currentMoves[i] = moveCoordinates;
            currentDiceTransform
        }
        return;
    }

    (List<(int, int)>, Transform) GetDiceInfo() {
        GameObject dice = deck.RollTheDie();
        Transform diceTransform = dice.GetComponent<Transform>();
        List<Vector2Int> movePackage = dice.GetComponent<ValidMoves>().GetMoves();
        
        (int, int) move;
        List<(int, int)> moveset = new List<(int, int)>();
        foreach (Vector2Int movePack in movePackage) {
            move = (movePack.x, movePack.y);
            moveset.Add(move);
        }
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
