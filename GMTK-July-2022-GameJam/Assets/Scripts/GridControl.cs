using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
// Defines the attributes and logic of the grid system. Currently limited to square grid/tiles.
// Dimensions must be manually altered in this script.
{
    // Manual-Entry information
    // private static Vector3 _originPosition = new Vector3(0, 0, 0); // World location of grid center
    // private static int _gridWorldSize = 40; // Grid dimensions in UnityUnits
    private static int _gridTileWidth = 4; // Number of tiles per side

    // Generated grid information
    private (int, int)[] _tileCoordinates;
    // private Vector3[] _tilePositions; // Corresponding tile world positions

    public (int, int) _player1Position;
    public (int, int) _player2Position;

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

        // // Generate Unity world locations
        // Vector3 gridNegXZCorner = new Vector3(_originPosition.x - _gridWorldSize/2, _originPosition.y, _originPosition.z - _gridWorldSize/2);
        // _tilePositions = new Vector3[_tileCoordinates.Length];
        // index = 0;
        // for (float x = 0.5f; x < _gridTileWidth; x++) {
        //     for (float z = 0.5f; z < _gridTileWidth; z++) {
        //         _tilePositions[index] = new Vector3(gridNegXZCorner.x + (x * tileWorldSize), gridNegXZCorner.y, gridNegXZCorner.z + (z * tileWorldSize));
        //         index++;
        //     }
        // }
    }

    List<(int, int)> GetMoveCoordinates(Vector2Int playerCoordinates, List<Vector2Int> offsets) {
        int playerX, int playerZ = playerCoordinates.x, playerCoordinates.y;

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

