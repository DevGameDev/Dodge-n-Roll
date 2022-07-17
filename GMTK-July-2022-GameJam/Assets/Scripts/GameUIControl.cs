using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIControl : MonoBehaviour
{
    // Settings
    Vector3 hiddenPosition = new Vector3(0, 0, 0);

    private Transform tileCoordinates = GameObject.Find("gridCoordinates").GetComponent<Transform>();
    private Transform tileOptions = GameObject.Find("tileOptions").GetComponent<Transform>();
    // private Transform tileHighlights = GameObject.Find("gridHightlightsPositions").GetComponent<Transform>();
    private Transform diePiecePositions = GameObject.Find("dicePositions").GetComponent<Transform>();
    private Transform dieSprites = GameObject.Find("Deck").GetComponent<Transform>();

    private ((int, int), SpriteRenderer)[] tilesBasic = new ((int, int), SpriteRenderer)[GameControl.gridSize]; // ((x, y), (characterSprite, tileHighlightSprite))
    private ((int, int), (SpriteRenderer, SpriteRenderer))[] tiles = new ((int, int), (SpriteRenderer, SpriteRenderer))[GameControl.gridSize]; // ((x, y), (characterSprite, tileHighlightSprite))

    private Transform[] dieLocations = new Transform[GameControl.numDie];
    private SpriteRenderer[] arrowLocations = new SpriteRenderer[GameControl.numDie];
    private SpriteRenderer[] shadowLocations = new SpriteRenderer[GameControl.numDie];

    public void HandleGameLoad() {
        // Generate Game Positions
        int tileIndex = 0, dieIndex = 0, arrowIndex = 0, shadowIndex = 0;
        foreach (Transform coordinate in tileCoordinates) {
            int x = Int32.Parse(coordinate.gameObject.name.Split(",")[0]); 
            int y = Int32.Parse(coordinate.gameObject.name.Split(",")[1]);
            SpriteRenderer tileSprite = coordinate.gameObject.GetComponent<SpriteRenderer>();
            tilesBasic[tileIndex] = ((x, y), tileSprite);
            tileIndex++;
        }

        foreach (Transform diePiece in diePiecePositions) {
            string pieceName = diePiece.gameObject.name;
            if (pieceName.StartsWith("dice")) {
                dieLocations[dieIndex] = diePiece.GetComponent<Transform>();
                dieIndex++;
                continue;
            }
            else if (pieceName.StartsWith("arrow")) {
                arrowLocations[arrowIndex] = diePiece.GetComponent<SpriteRenderer>();
                arrowIndex++;
            }
            else if (pieceName.StartsWith("diceShadow")) {
                shadowLocations[shadowIndex] = diePiece.GetComponent<SpriteRenderer>();
            }
        }

        foreach (Transform dieSprite in dieSprites) {
            dieSprite.position = hiddenPosition;
        }
        // TODO: Display game start overlay
        return;
    }

    public void HandleGameStart() {
        // TODO: Clear Game start overlay
        return;
    }

    public void HideDie(Transform[] dieTransform) {
        for (int i = 0; i < GameControl.numDie; i++) {
            dieTransform[i].position = hiddenPosition;
        }
    }

    public void DisplayDie(bool[] activeDie, Transform[] dieTransform) {
        for (int i = 0; i < GameControl.numDie; i++) {
            dieTransform[i].position = dieLocations[i].position;
        }
        return;
    }

    public void HandleHoverDie(int lastSelectedDie, int selectedDie) {
        
        // TODO: Move selection arrow
        // TODO: Make old highlighted tiles invisble
        // TODO: Make new highlighted tiles visble
        return;
    } 

    public void HandleSelectDie(int selectedDie) {
        // TODO: Increase shadow alpha
        // TODO: make selection arrow invisible
        // TODO: Special highlight for currently selected tile
        return;
    }

    public void HandleDeselectDie(int selectedDie) {
        // TODO: Decrease shadow alpha
        // TODO: Make selection arrow visible
        // TODO: remomve special highlight for selected tile
        return;
    }

    public void HandleHoverTile(int lastSelectedTile, int selectedTile) {
        // TODO: replace special highlight on old tile
        // TODO: replace standard highlight on new tile
        return;
    }

    public void HandleSelectTile(int selectedTile) {
        // TODO: remove all tile highlights
        // TODO: Poof animation and teleport character
        return;
    }
}
