using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    // ========== Variables ==========
    // Settings
    private Vector3 hiddenPosition = new Vector3(0, 0, 0);
    private int shadowAlphaBase = 75;
    private int shadowAlphaHovered = 255;

    // UI Collections
    private Transform tileCoordinates = GameObject.Find("gridCoordinates").GetComponent<Transform>();
    private Transform diePiecePositions = GameObject.Find("dicePositions").GetComponent<Transform>();
    private Transform dieSprites = GameObject.Find("Deck").GetComponent<Transform>();

    // UI Elements
    private Image startScreen = GameObject.Find("gameStart").GetComponent<Image>();
    private ((int, int), Image)[] tiles = new ((int, int), Image)[GameControl.gridSize*GameControl.gridSize];
    private Transform[] dieLocations = new Transform[GameControl.numDie];
    private Image[] arrows = new Image[GameControl.numDie];
    private Image[] shadows = new Image[GameControl.numDie];

    // Sprites
    private Sprite tileHighlight;
    private Sprite tileHighlightHovered;
    private Sprite cowLeft;
    private Sprite cowRight;
    private Sprite cowFront;
    private Sprite cowBack;

    private Sprite player1Sprite;
    private Sprite player2Sprite;

    // ========== Initialization ==========
    public void LoadGame() {
        // Find tile images
        int tileIndex = 0, dieIndex = 0, arrowIndex = 0, shadowIndex = 0;
        foreach (Transform coordinate in tileCoordinates) {
            int x = Int32.Parse(coordinate.gameObject.name.Split(",")[0]); 
            int y = Int32.Parse(coordinate.gameObject.name.Split(",")[1]);
            Image tile = coordinate.gameObject.GetComponent<Image>();
            tiles[tileIndex] = ((x, y), tile);
            tileIndex++;
        }

        // Get die piece info
        foreach (Transform diePiece in diePiecePositions) {
            string pieceName = diePiece.gameObject.name;
            // Die world locations
            if (pieceName.StartsWith("dice")) {
                dieLocations[dieIndex] = diePiece.GetComponent<Transform>();
                dieIndex++;
                continue;
            }
            // Arrow images
            else if (pieceName.StartsWith("arrow")) {
                arrows[arrowIndex] = diePiece.GetComponent<Image>();
                arrowIndex++;
            }
            // Shadow images
            else if (pieceName.StartsWith("diceShadow")) {
                shadows[shadowIndex] = diePiece.GetComponent<Image>();
            }
        }

        foreach (Transform dieSprite in dieSprites) {
            dieSprite.position = hiddenPosition;
        }
        startScreen.enabled = true;
        LoadSprites();

        player1Sprite = cowFront;
        player2Sprite = cowBack;
    }

    public void LoadSprites() {
        tileHighlight = Resources.Load<Sprite>("tileHightlight");
        tileHighlightHovered = Resources.Load<Sprite>("tileHighlightHovered");
        cowLeft = Resources.Load<Sprite>("cowLeft");
        cowRight = Resources.Load<Sprite>("cowRight");
        cowBack = Resources.Load<Sprite>("cowBack");
        cowFront = Resources.Load<Sprite>("cowFront");
    }

    // ========== State Transitions ==========

    public void StartGame() {
        startScreen.enabled = false;
    }

    public void DisplayDie(bool[] activeDie, GameObject[] die) {
        for (int i = 0; i < GameControl.numDie; i++) {
            die[i].transform.position = dieLocations[i].position;
            if (activeDie[i]) die[i].GetComponent<Image>().color = Color.white;
            else die[i].GetComponent<Image>().color = Color.black;
            DeselectDie(i);
        }
    }

    public void HoverDie(int lastHoveredDie, int hoveredDie, List<(int, int)> lastCoordinates, List<(int, int)> moveCoordinates) {
        arrows[lastHoveredDie].enabled = false;
        arrows[hoveredDie].enabled = true;

        HideTiles(lastCoordinates);
        ShowTiles(moveCoordinates);
    } 

    public void ShowTiles(List<(int, int)> tileCoordinates) {
        foreach (((int, int) tileCoordinate, Image tileImage) in tiles) {
            if (tileCoordinates.Contains(tileCoordinate)) {
                tileImage.sprite = tileHighlight;
                tileImage.enabled = true;
            }
        }
    }

    public void HideTiles(List<(int, int)> tileCoordinates) {
        foreach (((int, int) tileCoordinate, Image tileImage) in tiles) {
            if (tileCoordinates.Contains(tileCoordinate)) {
                tileImage.enabled = false;
            }
        }
    }

    public void SelectDie(int selectedDie, List<(int, int)> moveCoordinates) {
        Color tempColor = shadows[selectedDie].color;
        shadows[selectedDie].color = new Color(tempColor.r, tempColor.g, tempColor.b, shadowAlphaHovered);

        arrows[selectedDie].enabled = false;
        HoverTile(0, 0, moveCoordinates);
    }

    public void DeselectDie(int selectedDie) {
        Color tempColor = shadows[selectedDie].color;
        shadows[selectedDie].color = new Color(tempColor.r, tempColor.g, tempColor.b, shadowAlphaBase);

        arrows[selectedDie].enabled = false;
    }

    public void HideDie(Transform[] dieTransform) {
        for (int i = 0; i < GameControl.numDie; i++) {
            dieTransform[i].position = hiddenPosition;
        }
    }

    public void HoverTile(int lastHoveredTile, int hoveredTile, List<(int, int)> moveCoordinates) {
        (int, int) hoveredTileCoord = moveCoordinates[hoveredTile];
        (int, int) lastHoveredTileCoord = moveCoordinates[lastHoveredTile];
        foreach (((int, int) tileCoordinate, Image tileImage) in tiles) {
            if (tileCoordinate == lastHoveredTileCoord) tileImage.sprite = tileHighlight;
            if (tileCoordinate == hoveredTileCoord) tileImage.sprite = tileHighlightHovered;
        }
    }
    
    public void SelectTile(int playerTurn, (int, int) playerCoordinate, int selectedTile, List<(int, int)> moveCoordinates) {
        HideTiles(moveCoordinates);
        (int, int) moveCoordinate = moveCoordinates[selectedTile];
        Image playerImage = GetCoordinateImage(playerCoordinate);
        playerImage.enabled = false;
        Image moveImage = GetCoordinateImage(moveCoordinate);
        if (playerTurn == 1) moveImage.sprite = player1Sprite;
        else moveImage.sprite = player2Sprite;
    }


    // ========== Utility ==========

    private Image GetCoordinateImage((int, int) coordinate) {
        foreach (((int, int) tileCoordinate, Image tileImage) in tiles) {
            if (coordinate == tileCoordinate) return tileImage;
        }
        return null;
    }

}
