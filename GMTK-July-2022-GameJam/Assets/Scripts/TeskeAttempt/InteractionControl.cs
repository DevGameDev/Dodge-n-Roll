using System.Collections.Generic;
using UnityEngine;

public class InteractionControl : MonoBehaviour
{

    [SerializeField] Deck theDeck;

    private bool[] player1ActiveDie = new bool[3] { true, true, true };
    private List<Vector2Int>[] player1Moves = new List<Vector2Int>[3];
    private GameObject[] player1Dice = new GameObject[3];


    private bool[] player2ActiveDie = new bool[3] { true, true, true };
    private List<Vector2Int>[] player2Moves = new List<Vector2Int>[3];
    private GameObject[] player2Dice = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        player1Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
        player2Dice = new GameObject[] { theDeck.RollTheDie(), theDeck.RollTheDie(), theDeck.RollTheDie() };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectADie()
    {
        GameObject currentlySelectedDie = player1Dice[0];
    }
}
