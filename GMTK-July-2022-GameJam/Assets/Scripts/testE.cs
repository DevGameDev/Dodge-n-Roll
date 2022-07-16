using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testE : MonoBehaviour
{

    GameObject test;
    
    // Start is called before the first frame update
    void Start()
    {
        test = GameObject.Find("Scripts");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(test.GetComponent<GameControls>().inputEnum);
    }
}
