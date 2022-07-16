using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour
// Defines the attributes and logic of the grid system. Currently limited to square grid/tiles.
// Dimensions must be manually altered in this script.
{
    // Manual-Entry information
    private static Vector3 _originPosition = new Vector3(0, 0, 0); // World location of grid center
    private static int _gridWorldSize = 50; // Grid dimensions in UnityUnits
    private static int _tileWorldSize = 10; // Tile dimensions in UnityUnits
    private static int _gridTileWidth = 4; // Number of tiles per side

    // Generated grid information
    private (int, int)[] _tilePositions; // Tile coordinates
    private Vector2[] _worldPositions; // Corresponding tile world positions

    private (int, int) _player1Position;
    private (int, int) _player2Position;

    private List<Vector2Int> _possibleMoves;

    private int playerTurnNumber; // 1 = Player 1's turn, 2 = Player 2's turn

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTiles() {
        _tilePositions = new (int, int)[_gridTileWidth*_gridTileWidth];
        int index = 0;
        for (int x = 0; x < _gridTileWidth; x++) {
            for (int z = 0; z < _gridTileWidth; z++) {
                _tilePositions[index] = (x, z);
                index++;
            }
        }
    }

    // TODO
    (int, int)[] FindPossibleMoves((int, int)[] offsets) {
        return new (int, int)[0];
    }
}
