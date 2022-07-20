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
    private Color shadowBaseColor = new Color(1f, 1f, 1f, 0.6f);

    // UI Collections
    private Transform tileCoordinates;
    private Transform diePiecePositions;
    private Transform dieSprites;

    // UI Elements
    private Image startScreen;
    private Transform selectedPlayerShadow;
    private Transform playerSkullIcon;
    private Transform hoveredPlayerSkullIcon;
    private ((int, int), Vector3, Image)[] tiles = new ((int, int), Vector3, Image)[GameControl.gridSize*GameControl.gridSize];
    private Transform[] dieLocations = new Transform[GameControl.numDie];
    private Image[] arrows = new Image[GameControl.numDie];
    private Image[] shadows = new Image[GameControl.numDie];

    // Sprites
    public Sprite tileHighlight;
    public Sprite tileHighlightHovered;
    public Sprite cowLeft;
    public  Sprite cowRight;
    public Sprite cowFront;
    public Sprite cowBack;

    // ========== Initialization ==========

    public void LoadGame((int, int) p1Coordinate, (int, int) p2Coordinate) {
        tileCoordinates = GameObject.Find("gridCoordinates").GetComponent<Transform>();
        diePiecePositions = GameObject.Find("dicePositions").GetComponent<Transform>();
        dieSprites = GameObject.Find("Deck").GetComponent<Transform>();
        startScreen = GameObject.Find("gameStart").GetComponent<Image>();
        selectedPlayerShadow = GameObject.Find("selectedPlayerShadow").GetComponent<Transform>();
        playerSkullIcon = GameObject.Find("playerSkullIcon").GetComponent<Transform>();
        hoveredPlayerSkullIcon = GameObject.Find("hoveredPlayerSkullIcon").GetComponent<Transform>();

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

        // Get die piece info
        foreach (Transform diePiece in diePiecePositions) {
            string pieceName = diePiece.gameObject.name;
            // Shadow images
            if (pieceName.StartsWith("diceShadow")) {
                Image diceShadow = diePiece.GetComponent<Image>();
                diceShadow.color = shadowBaseColor;
                shadows[shadowIndex] = diceShadow;
                shadowIndex++;
                continue;
            }
            // Die world locations
            else if (pieceName.StartsWith("dice")) {
                dieLocations[dieIndex] = diePiece.GetComponent<Transform>();
                dieIndex++;
                continue;
            }
            // Arrow images
            else if (pieceName.StartsWith("arrow")) {
                arrows[arrowIndex] = diePiece.GetComponent<Image>();
                arrowIndex++;
                continue;
            }
            Image p1Image = GetCoordinateImage(p1Coordinate);
            p1Image.enabled = true;
            p1Image.sprite = cowBack;
            Image p2Image = GetCoordinateImage(p1Coordinate);
            p2Image.enabled = true;
            p2Image.sprite = cowFront;
        }

        foreach (Transform dieSprite in dieSprites) {
            dieSprite.position = hiddenPosition;
        }
        startScreen.enabled = true;

        // Spawn Players
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinate == p1Coordinate) {
                tileImage.sprite = cowRight;
                tileImage.enabled = true;
                tileImage.color = new Color(1f, 0.6f, 0.6f);
            }
            else if (tileCoordinate == p2Coordinate) {
                tileImage.sprite = cowLeft;
                tileImage.enabled = true;
            }
        }
    }

    // ========== State Transitions ==========

    public void StartGame() {
        startScreen.enabled = false;
    }

    public void DisplayDie(GameObject[] die, List<(int, int)>[] movesets, bool[] activeDie) {
        Image diceImage;
        for (int i = 0; i < GameControl.numDie; i++) {
            die[i].transform.position = dieLocations[i].position;
            diceImage = die[i].GetComponent<Image>();
            if (activeDie[i] && movesets[i].Count < 1) diceImage.color = Color.gray;
            else if (activeDie[i]) diceImage.color = Color.white;
            else {
                diceImage.color = Color.black;
                shadows[i].enabled = false;
                arrows[i].enabled = false;
            }
            ResetDie(i);
        }
    }

    public void HoverDie(int lastHoveredDie, int hoveredDie, List<(int, int)> lastCoordinates, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        arrows[lastHoveredDie].enabled = false;
        arrows[hoveredDie].enabled = true;

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
        shadows[selectedDie].color = Color.white;
        arrows[selectedDie].enabled = false;
        HoverTile(0, 0, moveCoordinates, opposingPlayerCoordinate);
    }

    private void ResetDie(int selectedDie) {
        shadows[selectedDie].color = shadowBaseColor;
        arrows[selectedDie].enabled = false;
    }

    public void DeselectDie(int selectedDie, int hoveredTile, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        shadows[selectedDie].color = shadowBaseColor;
        arrows[selectedDie].enabled = true;
        UnhoverTile(hoveredTile, moveCoordinates, opposingPlayerCoordinate);
    }

    public void HideDie(GameObject[] die) {
        for (int i = 0; i < GameControl.numDie; i++) {
            die[i].transform.position = hiddenPosition;
        }
    }

    public void HoverTile(int lastHoveredTile, int hoveredTile, List<(int, int)> moveCoordinates, (int, int) opposingPlayerCoordinate) {
        (int, int) hoveredTileCoord = moveCoordinates[hoveredTile];
        (int, int) lastHoveredTileCoord = moveCoordinates[lastHoveredTile];
        foreach (((int, int) tileCoordinate, Vector3 tilePosition, Image tileImage) in tiles) {
            if (tileCoordinate == lastHoveredTileCoord) {
                if (tileCoordinate == opposingPlayerCoordinate) hoveredPlayerSkullIcon.position = hiddenPosition;
                else tileImage.sprite = tileHighlight;
            }
            else if (tileCoordinate == hoveredTileCoord) {
                if (tileCoordinate == opposingPlayerCoordinate) {
                    playerSkullIcon.position = hiddenPosition;
                    hoveredPlayerSkullIcon.position = tilePosition;
                }
                else tileImage.sprite = tileHighlightHovered;
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
        playerImage.color = Color.white;
        playerImage.enabled = false;
        Sprite playerSprite = GetMovePlayerSprite(playerCoordinate, moveCoordinate);
        Image moveImage = GetCoordinateImage(moveCoordinate);
        moveImage.enabled = true;
        moveImage.sprite = playerSprite;
        moveImage.color = Color.white;
        Image opposingImage = GetCoordinateImage(opposingPlayerCoordinate);
    }

    // ========== Utility ==========

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

    private Sprite GetMovePlayerSprite((int, int) playerCoordinate, (int, int) moveCoordinate) {
        MoveDirection direction = GetMoveDirection(playerCoordinate, moveCoordinate);
        if (direction == MoveDirection.Left) return cowLeft;
        else if (direction == MoveDirection.Right) return cowRight;
        else if (direction == MoveDirection.Back) return cowBack;
        else return cowFront;
    }

    private MoveDirection GetMoveDirection((int, int) playerCoordinate, (int, int) moveCoordinate) {
        (int playerX, int playerY) = playerCoordinate;
        (int moveX, int moveY) = moveCoordinate;
        // TODO: fix some coords
        int xDistance = Math.Abs(playerX - moveX);
        int yDistance = Math.Abs(playerY - moveY);

        if (xDistance > 0 && yDistance == 0) {
            return MoveDirection.Right;
        }
        else if (xDistance < 0 && yDistance == 0) {
            return MoveDirection.Left;
        }
        else if (xDistance == 0 && yDistance < 0) {
            return MoveDirection.Back;
        }
        else if (xDistance == 0 && yDistance > 0) {
            return MoveDirection.Front;
        }
        else if (xDistance > 0 && yDistance > 0) {
            if (xDistance < yDistance) return MoveDirection.Front;
            return MoveDirection.Right;
        }
        else if (xDistance < 0 && yDistance < 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
            return MoveDirection.Back;
        }
        else if (xDistance < 0 && yDistance > 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
            return MoveDirection.Front;
        }
        else if (xDistance > 0 && yDistance < 0) {
            if (xDistance < yDistance) return MoveDirection.Left;
        }
        return MoveDirection.Front;
    }

}
