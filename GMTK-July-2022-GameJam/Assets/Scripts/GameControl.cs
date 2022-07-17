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
        TileSelection,
        Moving,
        GameOver,
    }

    // The Deck
    [SerializeField] private Deck theDeck;

    // Settings
    public static int numDie = 3; // Number of dice that are rolled at once
    public static int gridSize = 4; // Number of tiles in each dimension

    // GameObject / Modules
    GameObject gameController;
    private InputControl controls;
    private GameUIControl ui;
    private GridControl grid;

    // State
    private int playerTurn; // 1 = Player 1 turn, 2 = Player 2 turn
    private GameStates currentState;
    private InputStates currentInput;

    private int lastSelectedDie = 0;
    private int selectedDie = 0; // selected die/move
    private int lastSelectedTile = 0;
    private int selectedTile = 0; // index of coordinate in move

    private bool[] currentActiveDie;
    private GameObject[] currentDice;
    private List<Vector2Int>[] currentMoves;
    
    private bool[] player1ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] player1Dice = new GameObject[3];
    private List<Vector2Int>[] player1Moves = new List<Vector2Int>[numDie];

    private bool[] player2ActiveDie = Enumerable.Repeat(true, numDie).ToArray();
    private GameObject[] player2Dice = new GameObject[3];
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
        player1Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
        player2Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
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

    void RunDiceSelection() {
        currentState = GameStates.DiceSelection;
        if (playerTurn == 1) {
            currentActiveDie = player1ActiveDie;
            currentDice = player1Dice;
            currentMoves = player1Moves;
        }
        else {
            currentActiveDie = player2ActiveDie;
            currentDice = player2Dice;
            currentMoves = player2Moves;
        }
        // TODO: Generate Dice for each player if necessary
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
}

