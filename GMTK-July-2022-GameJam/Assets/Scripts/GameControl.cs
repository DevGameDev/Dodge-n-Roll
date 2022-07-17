using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // ========== Variables ==========

    // Settings
    public static int numDie = 3; // Number of dice that are rolled at once
    public static int gridSize = 4; // Number of tiles in each dimension

    // GameObject / Modules
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

    private int lastHoveredDie = 0;
    private int hoveredDie = 0; // selected die/move
    private int lastHoveredTile = 0;
    private int hoveredTile = 0; // index of coordinate in move

    private bool[] currentActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] currentDie = new GameObject[numDie];
    private List<(int, int)>[] currentMoves = new List<(int, int)>[numDie];

    // Player States
    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] player1Die = new GameObject[numDie];
    private List<(int, int)>[] player1Moves = new List<(int, int)>[numDie];

    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] player2Die = new GameObject[numDie];
    private List<(int, int)>[] player2Moves = new List<(int, int)>[numDie];

    // ========== Initialization ==========

    void Start()
    {
        // Connect modules
        GameObject gameController = GameObject.Find("GameController");
        controls = gameController.GetComponent<InputControl>();
        ui = gameController.GetComponent<GameUIControl>();
        grid = gameController.GetComponent<GridControl>();

        // Set state information
        currentState = GameStates.GameStart;
        currentInput = InputStates.Idle;
        playerTurn = 1;

        // Generate grid and UI elements
        grid.GenerateTiles();
        ui.LoadGame();
    }

    // ========== Game Loop ==========

    void Update()
    {
        currentInput = controls.ProcessInput();
        if (currentInput == InputStates.Exit) ExitGame(); // Exit closes application in any state
        else if (currentState == GameStates.GameStart) GameStartTick(currentInput);
        else if (currentState == GameStates.DiceSelection) DiceSelectionTick(currentInput);
        else if (currentState == GameStates.TileSelection) TileSelectionTick(currentInput);
    }

    void GameStartTick(InputStates input) {
        if (input == InputStates.Enter) {
            ui.StartGame();
            RunDiceSelection();
        }
    }

    void DiceSelectionTick(InputStates input) {
        lastHoveredDie = hoveredDie;
        if (input == InputStates.Enter) {
            RunTileSelection();
        }
        else if (input == InputStates.Left) {
            hoveredDie--;
            while (hoveredDie < 0 || !currentActiveDie[hoveredDie]) {
                hoveredDie--;
                if (hoveredDie < 0) hoveredDie = numDie-1;
            }
            ui.HoverDie(lastHoveredDie, hoveredDie, currentMoves[hoveredDie]);
        }
        else if (input == InputStates.Right) {
            hoveredDie++;
            while (hoveredDie >= numDie || !currentActiveDie[hoveredDie]) {
                hoveredDie++;
                if (hoveredDie >= 0) hoveredDie = 0;
            }
            ui.HoverDie(lastHoveredDie, hoveredDie, currentMoves[hoveredDie]);
        }
    }

    void TileSelectionTick(InputStates input) {
        lastHoveredTile = hoveredTile;
        if (input == InputStates.Enter) {
            RunMove();
        }
        else if (input == InputStates.Back) {
            RunDiceSelection();
        }
        else if (input == InputStates.Left) {
            hoveredTile--;
            while (hoveredDie < 0 || !currentActiveDie[hoveredDie]) {
                hoveredDie--;
                if (hoveredDie < 0) hoveredDie = numDie-1;
            }
            if (lastHoveredDie != hoveredDie) ui.HoverTile(lastHoveredDie, hoveredDie, currentMoves[hoveredDie]);
        }
        else if (input == InputStates.Right) {
            hoveredDie++;
            while (hoveredDie >= numDie || !currentActiveDie[hoveredDie]) {
                hoveredDie++;
                if (hoveredDie >= 0) hoveredDie = 0;
            }
            if (lastHoveredDie != hoveredDie) ui.HoverTile(lastHoveredDie, hoveredDie, currentMoves[hoveredDie]);
        }
    }

    // ===== State Transitions =====

    void RunDiceSelection() {
        currentState = GameStates.DiceSelection;
        SetPlayerVariables();

        if (currentActiveDie.Count<bool>() <= 0) GenerateDie();
        for (int i = 0; i < numDie; i++) {
            if (currentActiveDie[i]) hoveredDie = i;
        }

        ui.DisplayDie(currentActiveDie, currentDie);
        ui.HoverDie(hoveredDie, hoveredDie, currentMoves[hoveredDie]);
    }

    void RunTileSelection() {
        currentState = GameStates.TileSelection;
        hoveredTile = 0;
        ui.SelectDie(hoveredDie, currentMoves[hoveredDie]);
    }

    void RunMove() {
        currentState = GameStates.Moving;
        ui.SelectTile(hoveredTile);
    }

    // ===== Utility =====

    void ExitGame() {
        Application.Quit();
    }

    void GenerateDie() {
        List<(int, int)> moveset = new List<(int, int)>();
        List<(int, int)> moveCoordinates = new List<(int, int)>();
        GameObject dice;
        bool successfulDice;
        for (int i = 0; i < numDie; i++) {
            currentActiveDie[i] = true;
            moveset.Clear();
            moveCoordinates.Clear();

            successfulDice = false;
            while (!successfulDice) {
                (moveset, dice) = GetDiceInfo();
                moveCoordinates = grid.GetValidMoveCoordinates(playerTurn, moveset);
                if (moveCoordinates.Count < 1) continue;
                currentMoves[i] = moveCoordinates;
                currentDie[i] = dice;
                successfulDice = true;
            }
        }
    }

    (List<(int, int)>, GameObject) GetDiceInfo() {
        GameObject dice = deck.RollTheDie();
        List<Vector2Int> movePackage = dice.GetComponent<ValidMoves>().GetMoves();
        
        (int, int) move;
        List<(int, int)> moveset = new List<(int, int)>();
        foreach (Vector2Int movePack in movePackage) {
            move = (movePack.x, movePack.y);
            moveset.Add(move);
        }
        return (moveset, dice);
    }

    void SetPlayerVariables() {
        if (playerTurn == 1) {
            currentActiveDie = player1ActiveDie;
            currentDie = player1Die;
            currentMoves = player1Moves;
        }
        else {
            currentActiveDie = player2ActiveDie;
            currentDie = player2Die;
            currentMoves = player2Moves;
        }
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
