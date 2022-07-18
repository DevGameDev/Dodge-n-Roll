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
    private InputControl CONTROLS;
    private GameUIControl UI;
    private Grid GRID;
    private Deck deck;

    // State
    public enum GameStates { // each automatically assigned a value on 0-(len-1)
        GameStart,
        DiceSelection,
        TileSelection,
        Moving,
        GameOver,
    }

    private GameStates currentState;
    private int playerTurn; // 1 = Player 1 turn, 2 = Player 2 turn
    private InputStates currentInput;

    private int lastHoveredDieIndex = 0;
    private int hoveredDieIndex = 0; 
    private int lastHoveredTileIndex = 0;
    private int hoveredTileIndex = 0;

    private (int, int) currentPlayerCoordinate;
    private (int, int) opposingPlayerCoordinate;
    private bool[] currentActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] currentDieObject = new GameObject[numDie];
    private List<(int, int)>[] currentValidMoveListArray = new List<(int, int)>[numDie];

    // Player States
    private (int, int) p1Coordinate = (0, 0);
    private bool[] p1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] p1DiceObjects = new GameObject[numDie];
    private List<(int, int)>[] p1MoveListArray = new List<(int, int)>[numDie];

    private (int, int) p2Coordinate = (numDie-1, numDie-1);
    private bool[] p2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] p2DiceObjects = new GameObject[numDie];
    private List<(int, int)>[] p2MoveListArray = new List<(int, int)>[numDie];

    // ========== Initialization ==========

    void Start()
    {
        // Connect modules
        GameObject gameControllerObject = GameObject.Find("GameController");
        CONTROLS = gameControllerObject.GetComponent<InputControl>();
        UI = gameControllerObject.GetComponent<GameUIControl>();
        GRID = gameControllerObject.GetComponent<Grid>();

        // Set state information
        currentState = GameStates.GameStart;
        currentInput = InputStates.Idle;
        playerTurn = 2;

        // Generate grid and UI elements
        GRID.GenerateTiles();
        UI.LoadGame();
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

    void GameStartTick(InputStates input) {
        if (input != InputStates.Idle) {
            if (playerTurn == 1) playerTurn = 2;
            else playerTurn = 1;
            UI.StartGame();
            StartDiceSelection();
        }
    }

    // ========== Dice Selection ==========
    void StartDiceSelection() {
        currentState = GameStates.DiceSelection;
        SetPlayerVariables();

        if (currentActiveDie.Count<bool>() <= 0) GenerateDie();
        for (int i = 0; i < numDie; i++) {
            if (currentActiveDie[i]) {
                hoveredDieIndex = i;
                break;
            }
        }
        List<(int, int)> noCoordinates = new List<(int, int)>();
        UI.DisplayDie(currentActiveDie, currentDieObject);
        UI.HoverDie(hoveredDieIndex, hoveredDieIndex, noCoordinates, currentValidMoveListArray[hoveredDieIndex]);
    }

    void DiceSelectionTick(InputStates input) {
        lastHoveredDieIndex = hoveredDieIndex;
        if (input == InputStates.Enter) {
            StartTileSelection();
        }
        else if (input == InputStates.Left) {
            hoveredDieIndex--;
            while (hoveredDieIndex < 0 || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex--;
                if (hoveredDieIndex < 0) hoveredDieIndex = numDie-1;
            }
            UI.HoverDie(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[lastHoveredDieIndex], currentValidMoveListArray[hoveredDieIndex]);
        }
        else if (input == InputStates.Right) {
            hoveredDieIndex++;
            while (hoveredDieIndex >= numDie || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex++;
                if (hoveredDieIndex >= 0) hoveredDieIndex = 0;
            }
            UI.HoverDie(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[lastHoveredDieIndex], currentValidMoveListArray[hoveredDieIndex]);
        }
    }

    // ========== Tile Selection ==========

    void StartTileSelection() {
        currentState = GameStates.TileSelection;
        hoveredTileIndex = 0;
        UI.SelectDie(hoveredDieIndex, currentValidMoveListArray[hoveredDieIndex]);
    }

    void TileSelectionTick(InputStates input) {
        lastHoveredTileIndex = hoveredTileIndex;
        if (input == InputStates.Enter) {
            EndTurn();
        }
        else if (input == InputStates.Back) {
            StartDiceSelection();
        }
        else if (input == InputStates.Left) {
            hoveredTileIndex--;
            while (hoveredDieIndex < 0 || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex--;
                if (hoveredDieIndex < 0) hoveredDieIndex = numDie-1;
            }
            if (lastHoveredDieIndex != hoveredDieIndex) UI.HoverTile(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[hoveredDieIndex]);
        }
        else if (input == InputStates.Right) {
            hoveredDieIndex++;
            while (hoveredDieIndex >= numDie || !currentActiveDie[hoveredDieIndex]) {
                hoveredDieIndex++;
                if (hoveredDieIndex >= 0) hoveredDieIndex = 0;
            }
            if (lastHoveredDieIndex != hoveredDieIndex) UI.HoverTile(lastHoveredDieIndex, hoveredDieIndex, currentValidMoveListArray[hoveredDieIndex]);
        }
    }

    void EndTurn() {
        currentState = GameStates.Moving;
        UI.SelectTile(playerTurn, currentPlayerCoordinate, hoveredTileIndex, currentValidMoveListArray[hoveredDieIndex]);
        currentPlayerCoordinate = currentValidMoveListArray[hoveredDieIndex][hoveredTileIndex];
        if (currentPlayerCoordinate == opposingPlayerCoordinate) EndRound(playerTurn);
        else {
            SwitchPlayerTurn();
            StartDiceSelection();
        }
    }

    void EndRound(int playerWinner) {
        if (playerWinner == 1) Debug.Log("Player 1 wins the round!");
        else Debug.Log("Player 2 wins the round!");
        return;
    }

    // ===== End Game Loop =====

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
                moveCoordinates = GRID.GetValidMoveCoordinates(currentPlayerCoordinate, moveset);
                if (moveCoordinates.Count < 1) continue;
                currentValidMoveListArray[i] = moveCoordinates;
                currentDieObject[i] = dice;
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

    void SwitchPlayerTurn() {
        if (playerTurn == 1) playerTurn = 2;
        else playerTurn = 1;
    }
    void SetPlayerVariables() {
        if (playerTurn == 1) {
            currentPlayerCoordinate = p1Coordinate;
            opposingPlayerCoordinate = p2Coordinate;
            currentActiveDie = p1ActiveDie;
            currentDieObject = p1DiceObjects;
            currentValidMoveListArray = p1MoveListArray;
        }
        else {
            currentPlayerCoordinate = p2Coordinate;
            opposingPlayerCoordinate = p1Coordinate;
            currentActiveDie = p2ActiveDie;
            currentDieObject = p2DiceObjects;
            currentValidMoveListArray = p2MoveListArray;
        }
    }
}