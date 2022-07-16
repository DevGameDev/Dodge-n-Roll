using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMoves : MonoBehaviour
{
    public List<Vector2Int> moves;

    public List<Vector2Int> GetMoves()
    {
        return moves;
    }
}
