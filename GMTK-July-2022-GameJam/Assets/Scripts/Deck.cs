using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    List<GameObject> listOfChildren;
    Transform dieFace;

    public void Start()
    {
        foreach (Transform child in transform)
        {
            listOfChildren.Add(child.gameObject);
        }

        RollTheDie();
    }

    public GameObject RollTheDie()
    {
        GameObject resultingDie;

        // Get number of child objects, choose a random number from 1 to that number
        int numberOfChildren = listOfChildren.Count;
        int randomPick = Random.Range(1, numberOfChildren);

        // Set current die face to the random number chosen
        dieFace = listOfChildren[randomPick].transform;

        // Create a list of the children of that die face
        List<GameObject> listOfGrandchildren = new List<GameObject>();
        foreach (Transform child in dieFace)
        {
            listOfGrandchildren.Add(child.gameObject);
        }

        // Choose one of those children, then return the associated GameObject
        numberOfChildren = listOfGrandchildren.Count;
        randomPick = Random.Range(1, numberOfChildren);
        resultingDie = listOfGrandchildren[randomPick];

        Debug.Log(resultingDie);
        return resultingDie;
    }

}
