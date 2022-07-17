using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
// Defines the attributes and logic of the grid system. Currently limited to square grid/tiles.
// Dimensions must be manually altered in this script.
{
    // Manual-Entry information
    private static int _gridTileWidth = 4; // Number of tiles per side

    // Generated grid information
    private (int, int)[] _tileCoordinates;
    public (int, int) _player1Position = (0, 0);
    public (int, int) _player2Position = (_gridTileWidth-1, _gridTileWidth-1);

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

    List<(int, int)> GetMoveCoordinates((int, int) playerCoordinates, List<(int, int)> offsets) {
        (int playerX, int playerZ) = playerCoordinates;

        List<(int, int)> moveCoordinates = new List<(int, int)>();
        foreach ((int moveX, int moveZ) in offsets) {
            int moveCoordinateX = (playerX - moveX);
            if (moveCoordinateX < 0 || moveCoordinateX > _gridTileWidth) continue;

            int moveCoordinateZ = (playerZ - moveZ);
            if (moveCoordinateZ < 0 || moveCoordinateZ > _gridTileWidth) continue;

            moveCoordinates.Add((moveCoordinateX, moveCoordinateZ));
        }
        return moveCoordinates;
    }

    bool isValidMove(List<(int, int)> moveCoordinates) {
        if (moveCoordinates.Count > 0) return true;
        return false;
    }

    // bool ExecuteMove((int, int) playerCoordinates, List<(int, int)> moveCoordinates) {
    //     if (!HasValidMove(moveCoordinates)) {
    //     }
    // }
}

