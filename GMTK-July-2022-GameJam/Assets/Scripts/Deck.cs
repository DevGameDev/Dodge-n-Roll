using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public List<GameObject> listOfChildren;
    Transform dieFace;

    public void Start()
    {
        //Test Code, delete later
        RollTheDie();
        RollTheDie();
        RollTheDie();
        RollTheDie();
        RollTheDie();
    }

    public GameObject RollTheDie()
    {
        GameObject resultingDie;

        // Get number of child objects, choose a random number from 1 to that number
        int numberOfChildren = listOfChildren.Count;
        int randomPick = Random.Range(0, numberOfChildren);
        Debug.Log("Child picked = " + randomPick);

        // Set current die face to the random number chosen
        dieFace = listOfChildren[randomPick].transform;

        // Create a list of the children of that die face
        List<GameObject> listOfGrandchildren = dieFace.GetComponent<DeckChild>().listOfGrandChildren;

        // Choose one of those children, then return the associated GameObject
        numberOfChildren = listOfGrandchildren.Count;
        randomPick = Random.Range(0, numberOfChildren);
        Debug.Log("Grandchild picked = " + randomPick);
        resultingDie = listOfGrandchildren[randomPick];

        Debug.Log(resultingDie);
        return resultingDie;
    }

    // Todo 
    public void PlayDie()
    {

    }

}
