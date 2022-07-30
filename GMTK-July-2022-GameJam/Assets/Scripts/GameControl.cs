/*
Primary game control for Dodge 'n Roll. Contains the gameplay loop and logic for all game mechanics. Methods called 
during the loop handle graphics and UI through GameUiControl. Input is handled through InputControl. 

Loop runs as follows: GameControl.Update() calls the appropriate "tick" function per the value of "currentState"
that handles input and delegates calls. Given an "enter" input, a state transition is initiated through the 
corresponding "start" function, which handles all logic and UI pertaining to the switch. 

Bounties: 
- The moveset needs to be updated, gameplay is too predictable. (Devin has an idea)
- On game end, the scene is simply reset. A fix likely includes:
    - A game-end UI
    - A system for multiple rounds
    - Saving previous game results
- Different sprites for each player
- Music
- Sound effects
- Show both player's die on either side of board, instead of one at a time.
*/


using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    // Settings
    public static int NUMDIE = 3; // Number of die rolled at once
    public static int GRIDSIZE = 4; // Number of tiles of grid in each dimension

    // Modules
    private InputControl CONTROLS;
    private GameUIControl UI;
    private Grid GRID;
    private Deck DECK;

    // State
    public enum GameStates {
        GameStart,
        DiceSelection,
        TileSelection,
        Moving,
        GameOver,
    }

    private GameStates currentState;
    private InputStates currentInput; // Enum in InputControl
    private int playerTurn = 1; // 1 = Player 1 turn, 2 = Player 2 turn

    private int lastHoveredDieIndex = 0;
    private int hoveredDieIndex = 0; 
    private int lastHoveredTileIndex = 0;
    private int hoveredTileIndex = 0;

    private (int, int) currentPlayerCoordinate;
    private (int, int) opposingPlayerCoordinate;
    private bool[] currentActiveDie = Enumerable.Repeat(false, NUMDIE).ToArray();
    private GameObject[] currentDiceObjects = new GameObject[NUMDIE];
    private List<(int, int)>[] currentValidMoveListArray = new List<(int, int)>[NUMDIE];

    // Player 1 state
    private (int, int) p1Coordinate = (0, 0);
    private bool[] p1ActiveDie = Enumerable.Repeat(false, NUMDIE).ToArray();
    private GameObject[] p1DiceObjects = new GameObject[NUMDIE];
    private List<(int, int)>[] p1ValidMoveListArray = new List<(int, int)>[NUMDIE];

    // Player 2 state
    private (int, int) p2Coordinate = (GRIDSIZE-1, GRIDSIZE-1);
    private bool[] p2ActiveDie = Enumerable.Repeat(false, NUMDIE).ToArray();
    private GameObject[] p2DiceObjects = new GameObject[NUMDIE];
    private List<(int, int)>[] p2ValidMoveListArray = new List<(int, int)>[NUMDIE];

    // Utility
    List<(int, int)> noCoordinate = new List<(int, int)>(); // Empty coordinate

    // ========== Initialization ==========

    void Start()
    {
        // Connect modules
        GameObject gameControllerObject = GameObject.Find("GameController");
        CONTROLS = gameControllerObject.GetComponent<InputControl>();
        UI = gameControllerObject.GetComponent<GameUIControl>();
        GRID = gameControllerObject.GetComponent<Grid>();
        DECK = GameObject.Find("Deck").GetComponent<Deck>();

        // Set state information
        currentState = GameStates.GameStart;
        currentInput = InputStates.Idle;

        // Generate grid and UI elements
        GRID.GenerateTiles();
        UI.LoadGame(p1Coordinate, p2Coordinate);
    }

    // ========== Game Loop ==========

    void Update()
    {
        currentInput = CONTROLS.ProcessInput();
        if (currentInput == InputStates.Exit) ExitGame(); // Exit closes application in any state
        else if (currentState == GameStates.GameStart) GameStartTick(currentInput);
        else if (currentState == GameStates.DiceSelection) DiceSelectionTick(currentInput);
        else if (currentState == GameStates.TileSelection) TileSelectionTick(currentInput);
    }

    // ========== Game Start ==========

    void GameStartTick(InputStates input) {
        if (input != InputStates.Idle) {
            UI.StartGame();
            StartDiceSelection();
        }
    }

    // ========== Dice Selection ==========

    void StartDiceSelection() {
        currentState = GameStates.DiceSelection;
        GetPlayerVariables();

        CheckDie();
        UI.SetPlayerShadows(currentPlayerCoordinate, opposingPlayerCoordinate);
        UI.DisplayDie(currentDiceObjects, currentValidMoveListArray, currentActiveDie);
        UI.HoverDie(hoveredDieIndex, hoveredDieIndex, noCoordinate, currentValidMoveListArray[hoveredDieIndex], opposingPlayerCoordinate);
    }

    void DiceSelectionTick(InputStates input) {
        lastHoveredDieIndex = hoveredDieIndex;
        if (input == InputStates.Enter && currentValidMoveListArray.Length > 0) {
            StartTileSelection();
        }
        else if (input == InputStates.Left) {
            hoveredDieIndex--;
            while (hoveredDieIndex < 0 || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex--;
                if (hoveredDieIndex < 0) hoveredDieIndex = NUMDIE - 1;
            }
            UI.HoverDie(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[lastHoveredDieIndex], currentValidMoveListArray[hoveredDieIndex], opposingPlayerCoordinate);
        }
        else if (input == InputStates.Right) {
            hoveredDieIndex++;
            while (hoveredDieIndex >= NUMDIE || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex++;
                if (hoveredDieIndex >= NUMDIE) hoveredDieIndex = 0;
            }
            UI.HoverDie(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[lastHoveredDieIndex], currentValidMoveListArray[hoveredDieIndex], opposingPlayerCoordinate);
        }
    }

    // ========== Tile Selection ==========

    void StartTileSelection() {
        currentState = GameStates.TileSelection;
        hoveredTileIndex = 0;
        UI.SelectDie(hoveredDieIndex, currentValidMoveListArray[hoveredDieIndex], opposingPlayerCoordinate);
    }

    void TileSelectionTick(InputStates input) {
        lastHoveredTileIndex = hoveredTileIndex;
        List<(int, int)> currentTiles = currentValidMoveListArray[hoveredDieIndex];
        if (input == InputStates.Enter) {
            EndTurn();
        }
        else if (input == InputStates.Back) {
            UI.DeselectDie(hoveredDieIndex, hoveredTileIndex, currentValidMoveListArray[hoveredDieIndex], opposingPlayerCoordinate);
            currentState = GameStates.DiceSelection;
        }
        else if (input == InputStates.Left) {
            hoveredTileIndex--;
            if (hoveredTileIndex < 0) hoveredTileIndex = currentTiles.Count() - 1;
            if (lastHoveredTileIndex != hoveredTileIndex) UI.HoverTile(lastHoveredTileIndex, hoveredTileIndex, currentTiles, opposingPlayerCoordinate);
        }
        else if (input == InputStates.Right) {
            hoveredTileIndex++;
            if (hoveredTileIndex >= currentTiles.Count()) hoveredTileIndex = 0;
            if (lastHoveredTileIndex != hoveredTileIndex) UI.HoverTile(lastHoveredTileIndex, hoveredTileIndex, currentTiles, opposingPlayerCoordinate);
        }
    }

    void EndTurn() {
        currentState = GameStates.Moving;
        currentActiveDie[hoveredDieIndex] = false;
        (int, int) moveCoordinate = currentValidMoveListArray[hoveredDieIndex][hoveredTileIndex];
        UI.SelectTile(playerTurn, currentPlayerCoordinate, currentValidMoveListArray[hoveredDieIndex], moveCoordinate, opposingPlayerCoordinate);
        if (moveCoordinate == opposingPlayerCoordinate) EndRound(playerTurn);
        else {
            UI.HideDie(currentDiceObjects);
            currentPlayerCoordinate = moveCoordinate;
            SetPlayerVariables();
            SwitchPlayerTurn();
            StartDiceSelection();
        }
    }

    void EndRound(int playerWinner) {
        Debug.Log(String.Format("Player {0} wins the round!", playerTurn));
        SceneManager.LoadScene("GameplayScene");
    }

    // ===== End Game Loop =====

    // ===== Utility =====


    void ExitGame() {
        Application.Quit();
    }

    void GenerateDie() {
        List<(int, int)> moveset = new List<(int, int)>();
        List<(int, int)> validMoves = new List<(int, int)>();
        GameObject dice;
        bool successfulDice;
        for (int i = 0; i < NUMDIE; i++) {
            currentActiveDie[i] = true;
            moveset.Clear();
            validMoves.Clear();

            successfulDice = false;
            while (!successfulDice) {
                dice = GetDice();
                moveset = GetDiceMoveset(dice);
                validMoves = GRID.GetValidMoveCoordinates(currentPlayerCoordinate, moveset);
                if (validMoves.Count < 1) continue;
                currentValidMoveListArray[i] = validMoves.ToList();
                currentDiceObjects[i] = dice;
                successfulDice = true;
            }
        }
    }

    void CheckDie() {
        bool hasValidDice = false;
        List<(int, int)> validMoves;
        for (int i = 0; i < NUMDIE; i++) {
            if (currentActiveDie[i]) {
                List<(int, int)> moveset = GetDiceMoveset(currentDiceObjects[i]);
                validMoves = GRID.GetValidMoveCoordinates(currentPlayerCoordinate, moveset);
                currentValidMoveListArray[i] = validMoves.ToList();
                if (validMoves.Count > 0) hasValidDice = true;
            }
        }
        if (!hasValidDice) GenerateDie();

        for (int i = 0; i < NUMDIE; i++) {
            if (currentActiveDie[i]) {
                hoveredDieIndex = i;
                break;
            }
        }
    }

    GameObject GetDice() {
        return DECK.RollTheDie();
    }

    List<(int, int)> GetDiceMoveset(GameObject dice) {
        List<Vector2Int> movePackage = dice.GetComponent<ValidMoves>().GetMoves();
        
        (int, int) move;
        List<(int, int)> moveset = new List<(int, int)>();
        foreach (Vector2Int movePack in movePackage) {
            move = (movePack.x, movePack.y);
            moveset.Add(move);
        }
        return moveset;
    }

    void SwitchPlayerTurn() {
        if (playerTurn == 1) playerTurn = 2;
        else playerTurn = 1;
    }

    void SetPlayerVariables() {
        // Saves "current" variables to corresponding player variables, based on current player turn`
        if (playerTurn == 1) {
            p1Coordinate = currentPlayerCoordinate;
            p2Coordinate = opposingPlayerCoordinate;
            p1ActiveDie = currentActiveDie;
            p1DiceObjects = currentDiceObjects;
            p1ValidMoveListArray = currentValidMoveListArray;
        }
        else {
            p2Coordinate = currentPlayerCoordinate;
            p1Coordinate = opposingPlayerCoordinate;
            p2ActiveDie  = currentActiveDie;
            p2DiceObjects = currentDiceObjects;
            p2ValidMoveListArray = currentValidMoveListArray;
        }
    }

    void GetPlayerVariables() {
        // Sets all "current" variables to reflect current player turn
        if (playerTurn == 1) {
            currentPlayerCoordinate = p1Coordinate;
            opposingPlayerCoordinate = p2Coordinate;
            currentActiveDie = p1ActiveDie;
            currentDiceObjects = p1DiceObjects;
            currentValidMoveListArray = p1ValidMoveListArray;
        }
        else {
            currentPlayerCoordinate = p2Coordinate;
            opposingPlayerCoordinate = p1Coordinate;
            currentActiveDie = p2ActiveDie;
            currentDiceObjects = p2DiceObjects;
            currentValidMoveListArray = p2ValidMoveListArray;
        }
    }
}