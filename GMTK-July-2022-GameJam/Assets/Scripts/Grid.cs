using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Manual-Entry information
    private static int _gridTileWidth = GameControl.gridSize; // Number of tiles per side

    // Generated grid information
    private (int, int)[] _tileCoordinates;

    public void GenerateTiles() {
        float tileWorldSize = _gridTileWidth / _gridTileWidth;

        // Generate int coordinate pairs
        _tileCoordinates = new (int, int)[_gridTileWidth*_gridTileWidth];
        int index = 0;
        for (int x = 0; x < _gridTileWidth; x++) {
            for (int z = 0; z < _gridTileWidth; z++) {
                _tileCoordinates[index] = (x, z);
                index++;
            }
        }
    }

    public List<(int, int)> GetValidMoveCoordinates((int, int) playerCoordinate, List<(int, int)> offsets) {
        int playerX, playerY;
        (playerX, playerY) = playerCoordinate;

        List<(int, int)> moveCoordinates = new List<(int, int)>();
        foreach ((int moveX, int moveY) in offsets) {
            int moveCoordinateX = (playerX - moveX);
            if (moveCoordinateX < 0 || moveCoordinateX > _gridTileWidth) continue;

            int moveCoordinateY = (playerY - moveY);
            if (moveCoordinateY < 0 || moveCoordinateY > _gridTileWidth) continue;

            if (moveCoordinateX == playerX && moveCoordinateY == playerY) continue;

            moveCoordinates.Add((moveCoordinateX, moveCoordinateY));
        }
        return moveCoordinates;
    }

    bool isValidMove(List<(int, int)> moveCoordinates) {
        if (moveCoordinates.Count > 0) return true;
        return false;
    }
}

