using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCtr : MonoBehaviour
{
    public Vector2 coord;
    public GameObject red;
    public GameObject black;

    GameObject overlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Destroy(overlay);
            overlay = Instantiate(red, transform.position, transform.rotation);
        }

        else if (Input.GetKeyDown(KeyCode.B))
        {
            Destroy(overlay);
            overlay = Instantiate(black, transform.position, transform.rotation);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy(overlay);
        }


    }
}
