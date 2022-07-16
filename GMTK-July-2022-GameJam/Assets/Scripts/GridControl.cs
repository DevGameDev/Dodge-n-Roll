using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
// Defines the attributes and logic of the grid system. Currently limited to square grid/tiles.
// Dimensions must be manually altered in this script.
{
    // Manual-Entry information
    private static Vector3 _originPosition = new Vector3(0, 0, 0); // World location of grid center
    private static int _gridWorldSize = 40; // Grid dimensions in UnityUnits
    private static int _gridTileWidth = 4; // Number of tiles per side

    // Generated grid information
    private int _tileWorldSize; // Tile dimensions in UnityUnits
    private (int, int)[] _tileCoordinates;
    private Vector3[] _tilePositions; // Corresponding tile world positions

    public (int, int) player1Position;
    public (int, int) player2Position;

    // Start is called before the first frame update
    void Start()
    {
        _tileWorldSize = _gridTileWidth / _gridTileWidth;
        GenerateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTiles() {
        _tileCoordinates = new (int, int)[_gridTileWidth*_gridTileWidth];
        int index = 0;
        for (int x = 0; x < _gridTileWidth; x++) {
            for (int z = 0; z < _gridTileWidth; z++) {
                _tileCoordinates[index] = (x, z);
                index++;
            }
        }
        _tilePositions = new Vector3[_tileCoordinates.Length];
        Vector3 gridNegXZCorner = new Vector3(_originPosition.x - _gridWorldSize/2, _originPosition.y, 
        for 
    }

    List<(int, int)> GetMoveCoordinates((int, int) playerCoordinates, (int, int)[] offsets) {
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

    bool HasValidMove(List<(int, int)> moveCoordinates) {
        if (moveCoordinates.Count > 0) return true;
        return false;
    }

    bool ExecuteMove((int, int) playerCoordinates, List<(int, int)> moveCoordinates) {
        if (!HasValidMove(moveCoordinates)) {
            if GameMaster.
            GameMaster.pl
        }
    }
}

