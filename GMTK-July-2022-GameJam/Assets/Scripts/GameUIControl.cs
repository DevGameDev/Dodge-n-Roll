using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIControl : MonoBehaviour
{
    // ========== Variables ==========
    // Settings
    private Vector3 hiddenPosition = new Vector3(-250, -250, -250);
    private Color shadowBaseColor = new Color(1f, 1f, 1f, 0.6f);
    private Vector3 shadowOffset = new Vector3(0, -50, 0);

    // UI Collections
    private Transform tileCoordinates;
    private Transform currentDiePiecePositions;
    private Transform p1DiePiecePositions;
    private Transform p2DiePiecePositions;

    private Transform currentDieSprites;
    private Transform p1DieSprites;
    private Transform p2DieSprites;

    // UI Elements
    private Image startScreen;
    private Transform currentPlayerShadow;
    private Transform opposingPlayerShadow;
    private Transform playerSkullIcon;
    private Transform hoveredPlayerSkullIcon;
    private ((int, int), Vector3, Image)[] tiles = new ((int, int), Vector3, Image)[GameControl.GRIDSIZE*GameControl.GRIDSIZE];
    private Transform[] currentDieLocations;
    private Transform[] p1DieLocations = new Transform[GameControl.NUMDIE];
    private Transform[] p2DieLocations = new Transform[GameControl.NUMDIE];
    private Image[] currentArrows;
    private Image[] p1Arrows = new Image[GameControl.NUMDIE];
    private Image[] p2Arrows = new Image[GameControl.NUMDIE];
    private Image[] currentShadows;
    private Image[] p1Shadows = new Image[GameControl.NUMDIE];
    private Image[] p2Shadows = new Image[GameControl.NUMDIE];

    // Sprites
    public Sprite tileHighlight;
    public Sprite tileHighlightHovered;
    public Sprite cowLeft;
    public Sprite cowRight;
    public Sprite cowFront;
    public Sprite cowBack;
    public Sprite pigLeft;
    public Sprite pigRight;
    public Sprite pigFront;
    public Sprite pigBack;

    // ========== Initialization ==========

    public void LoadGame((int, int) p1Coordinate, (int, int) p2Coordinate) {
        tileCoordinates = GameObject.Find("gridCoordinates").GetComponent<Transform>();
        p1DiePiecePositions = GameObject.Find("dicePositionsP1").GetComponent<Transform>();
        p2DiePiecePositions = GameObject.Find("dicePositionsP2").GetComponent<Transform>();
        p1DieSprites = GameObject.Find("DeckP1").GetComponent<Transform>();
        p2DieSprites = GameObject.Find("DeckP2").GetComponent<Transform>();
        startScreen = GameObject.Find("gameStart").GetComponent<Image>();
        currentPlayerShadow = GameObject.Find("currentPlayerShadow").GetComponent<Transform>();
        currentPlayerShadow.position = hiddenPosition;
        opposingPlayerShadow = GameObject.Find("opposingPlayerShadow").GetComponent<Transform>();
        opposingPlayerShadow.position = hiddenPosition;
        playerSkullIcon = GameObject.Find("playerSkullIcon").GetComponent<Transform>();
        playerSkullIcon.position = hiddenPosition;
        hoveredPlayerSkullIcon = GameObject.Find("hoveredPlayerSkullIcon").GetComponent<Transform>();
        hoveredPlayerSkullIcon.position = hiddenPosition;

        // Find tile images
        int tileIndex = 0, dieIndex = 0, arrowIndex = 0, shadowIndex = 0;
        foreach (Transform coordinate in tileCoordinates) {
            int x = Int32.Parse(coordinate.gameObject.name.Split(",")[0]); 
            int y = Int32.Parse(coordinate.gameObject.name.Split(",")[1]);
            Image tile = coordinate.gameObject.GetComponent<Image>();
            Vector3 position = coordinate.position;
            tile.enabled = false;
            tiles[tileIndex] = ((x, y), position, tile);
            tileIndex++;
        }

        // Get player 1 die piece info
        foreach (Transform diePiece in p1DiePiecePositions) {
            string pieceName = diePiece.gameObject.name;
            // Shadow images
            if (pieceName.StartsWith("diceShadow")) {
                Image diceShadow = diePiece.GetComponent<Image>();
                diceShadow.color = shadowBaseColor;
                p1Shadows[shadowIndex] = diceShadow;
                shadowIndex++;
                continue;
            }
            // Die world locations
            else if (pieceName.StartsWith("dice")) {
                p1DieLocations[dieIndex] = diePiece.GetComponent<Transform>();
                dieIndex++;
                continue;
            }
            // Arrow images
            else if (pieceName.StartsWith("arrow")) {
                p1Arrows[arrowIndex] = diePiece.GetComponent<Image>();
                arrowIndex++;
                continue;
            }
        }

        foreach (Transform dieSprite in p1DieSprites) {
            dieSprite.position = hiddenPosition;
        }
        startScreen.enabled = true;

        // Get player 2 die piece info
        dieIndex = 0; arrowIndex = 0; shadowIndex = 0;
        foreach (Transform diePiece in p2DiePiecePositions) {
            string pieceName = diePiece.gameObject.name;
            // Shadow images
            if (pieceName.StartsWith("diceShadow")) {
                Image diceShadow = diePiece.GetComponent<Image>();
                diceShadow.color = shadowBaseColor;
                p2Shadows[shadowIndex] = diceShadow;
                shadowIndex++;
                continue;
            }
            // Die world locations
            else if (pieceName.StartsWith("dice")) {
                p2DieLocations[dieIndex] = diePiece.GetComponent<Transform>();
                dieIndex++;
                continue;
            }
            // Arrow images
            else if (pieceName.StartsWith("arrow")) {
                p2Arrows[arrowIndex] = diePiece.GetComponent<Image>();
                arrowIndex++;
                continue;
            }
        }

        foreach (Transform dieSprite in p2DieSprites) {
            dieSprite.position = hiddenPosition;
        }
        startScreen.enabled = true;

        // Spawn Players
        Image p1Image = GetCoordinateImage(p1Coordinate);
        p1Image.enabled = true;
        p1Image.sprite = cowFront;
        Image p2Image = GetCoordinateImage(p2Coordinate);
        p2Image.enabled = true;
        p2Image.sprite = pigBack;

    }

    // ========== State Transitions ==========

    public void StartGame() {
        FadeToColor(startScreen, Color.white, Color.clear, 3);
        startScreen.enabled = false;
    }

    public void SetPlayerShadows((int, int) currentPlayerCoordinate, (int, int) opposingPlayerCoordinate) {
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinate == currentPlayerCoordinate) currentPlayerShadow.position = tilePosition + shadowOffset;
            if (tileCoordinate == opposingPlayerCoordinate) opposingPlayerShadow.position = tilePosition + shadowOffset;
        }
    }

    public void DisplayDie(GameObject[] die, List<(int, int)>[] movesets, bool[] activeDie) {
        Image diceImage;
        for (int i = 0; i < GameControl.NUMDIE; i++) {
            die[i].transform.position = currentDieLocations[i].position;
            diceImage = die[i].GetComponent<Image>();
            if (activeDie[i] && movesets[i].Count < 1) diceImage.color = Color.gray;
            else if (activeDie[i]) diceImage.color = Color.white;
            else {
                diceImage.color = Color.black;
                currentShadows[i].enabled = false;
                currentArrows[i].enabled = false;
            }
            ResetDie(i);
        }
    }

    public void HoverDie(int lastHoveredDie, int hoveredDie, List<(int, int)> lastCoordinates, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        currentArrows[lastHoveredDie].enabled = false;
        currentArrows[hoveredDie].enabled = true;

        HideTiles(lastCoordinates, opposingPlayerCoordinate);
        ShowTiles(moveCoordinates, opposingPlayerCoordinate);
    } 

    public void ShowTiles(List<(int, int)> tileCoordinates, (int, int) opposingPlayerCoordinate) {
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinates.Contains(tileCoordinate)) {
                if (tileCoordinate == opposingPlayerCoordinate) playerSkullIcon.position = tilePosition;
                else {
                    tileImage.sprite = tileHighlight;
                    tileImage.enabled = true;
                }
            }
        }
    }

    public void HideTiles(List<(int, int)> tileCoordinates, (int, int) opposingPlayerCoordinate) {
        playerSkullIcon.position = hiddenPosition;
        hoveredPlayerSkullIcon.position = hiddenPosition;
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinates.Contains(tileCoordinate)) {
                if (tileCoordinate == opposingPlayerCoordinate) continue;
                tileImage.enabled = false;
            }
        }
    }

    public void SelectDie(int selectedDie, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        currentShadows[selectedDie].color = Color.white;
        currentArrows[selectedDie].enabled = false;
        HoverTile(0, 0, moveCoordinates, opposingPlayerCoordinate);
    }

    private void ResetDie(int selectedDie) {
        currentShadows[selectedDie].color = shadowBaseColor;
        currentArrows[selectedDie].enabled = false;
    }

    public void DeselectDie(int selectedDie, int hoveredTile, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        currentShadows[selectedDie].color = shadowBaseColor;
        currentArrows[selectedDie].enabled = true;
        UnhoverTile(hoveredTile, moveCoordinates, opposingPlayerCoordinate);
    }

    public void HideDie(GameObject[] die) {
        for (int i = 0; i < GameControl.NUMDIE; i++) {
            die[i].transform.position = hiddenPosition;
        }
    }

    public void HoverTile(int lastHoveredTile, int hoveredTile, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        (int, int) hoveredTileCoord = moveCoordinates[hoveredTile];
        (int, int) lastHoveredTileCoord = moveCoordinates[lastHoveredTile];
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinate == hoveredTileCoord) {
                if (tileCoordinate == opposingPlayerCoordinate) {
                    playerSkullIcon.position = hiddenPosition;
                    hoveredPlayerSkullIcon.position = tilePosition;
                }
                else tileImage.sprite = tileHighlightHovered;
            }
            else if (tileCoordinate == lastHoveredTileCoord) {
                if (tileCoordinate == opposingPlayerCoordinate)
                {
                    hoveredPlayerSkullIcon.position = hiddenPosition;
                    playerSkullIcon.position = tilePosition;
                }
                else tileImage.sprite = tileHighlight;
            }
        }
    }

    public void UnhoverTile(int hoveredTile, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        (int, int) hoveredTileCoord = moveCoordinates[hoveredTile];
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinate == opposingPlayerCoordinate) hoveredPlayerSkullIcon.position = hiddenPosition;
            if (tileCoordinate == hoveredTileCoord) tileImage.sprite = tileHighlight;
        }
    }
    
    public void SelectTile(int playerTurn, (int, int) playerCoordinate, List<(int, int)> moveCoordinates, (int, int) moveCoordinate, (int, int) opposingPlayerCoordinate) {
        HideTiles(moveCoordinates, opposingPlayerCoordinate);
        Image playerImage = GetCoordinateImage(playerCoordinate);
        playerImage.enabled = false;
        Sprite playerSprite = GetMovePlayerSprite(playerCoordinate, moveCoordinate, playerTurn);
        Image moveImage = GetCoordinateImage(moveCoordinate);
        moveImage.enabled = true;
        moveImage.sprite = playerSprite;
        Image opposingImage = GetCoordinateImage(opposingPlayerCoordinate);
    }

    // ========== Utility ==========

    IEnumerator FadeToColor(Image image, Color start, Color end, float duration) {
        for (float t = 0f; t < duration; t += Time.fixedDeltaTime) {
            float normalizedTime = t/duration;
            image.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        image.color = end;
    }

    private Image GetCoordinateImage((int, int) coordinate) {
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (coordinate == tileCoordinate) return tileImage;
        }
        return null;
    }

    enum MoveDirection {
        Left,
        Right,
        Front,
        Back
    }

    private Sprite GetMovePlayerSprite((int, int) playerCoordinate, (int, int) moveCoordinate, int playerTurn) {
        MoveDirection direction = GetMoveDirection(playerCoordinate, moveCoordinate);
        if (playerTurn == 1)
        {
            if (direction == MoveDirection.Left) return cowLeft;
            else if (direction == MoveDirection.Right) return cowRight;
            else if (direction == MoveDirection.Back) return cowBack;
            else return cowFront;
        }
        else 
        {
            if (direction == MoveDirection.Left) return pigLeft;
            else if (direction == MoveDirection.Right) return pigRight;
            else if (direction == MoveDirection.Back) return pigBack;
            else return pigFront;
        }
    }

    private MoveDirection GetMoveDirection((int, int) playerCoordinate, (int, int) moveCoordinate) {
        (int playerX, int playerY) = playerCoordinate;
        (int moveX, int moveY) = moveCoordinate;
        // TODO: fix some coords
        int xDistance = moveX - playerX;
        int yDistance = moveY - playerY;

        if (xDistance > 0 && yDistance == 0) return MoveDirection.Right;
        if (xDistance < 0 && yDistance == 0) return MoveDirection.Left;
        if (xDistance == 0 && yDistance < 0) return MoveDirection.Back;
        if (xDistance == 0 && yDistance > 0) return MoveDirection.Front;
        if (xDistance > 0 && yDistance > 0) {
            if (xDistance < yDistance) return MoveDirection.Front;
            return MoveDirection.Right;
        }
        if (xDistance < 0 && yDistance < 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
            return MoveDirection.Back;
        }
        if (xDistance < 0 && yDistance > 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
            return MoveDirection.Front;
        }
        if (xDistance > 0 && yDistance < 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
        }
        return MoveDirection.Front;
    }

    public void GetPlayerUIElements(int playerTurn)
    {
        if (playerTurn == 1)
        {
            currentDiePiecePositions = p1DiePiecePositions;
            currentDieLocations = p1DieLocations;
            currentArrows = p1Arrows;
            currentShadows = p1Shadows;
            currentDieSprites = p1DieSprites;
        }
        else
        {
            currentDiePiecePositions = p2DiePiecePositions;
            currentDieLocations = p2DieLocations;
            currentArrows = p2Arrows;
            currentShadows = p2Shadows;
            currentDieSprites = p2DieSprites;
        }
    }

    public void SetPlayerUIElements(int playerTurn)
    {
        if (playerTurn == 1)
        {
            p1DiePiecePositions = currentDiePiecePositions;
            p1DieLocations = currentDieLocations;
            p1Arrows = currentArrows;
            p1Shadows = currentShadows;
            p1DieSprites = currentDieSprites;
        }
        else
        {
            p2DiePiecePositions = currentDiePiecePositions;
            p2DieLocations = currentDieLocations;
            p2Arrows = currentArrows;
            p2Shadows = currentShadows;
            p2DieSprites = currentDieSprites;
        }
    }
}
