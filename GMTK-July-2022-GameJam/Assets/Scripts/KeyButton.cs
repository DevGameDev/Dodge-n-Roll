using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{

    public void InvokeButton()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}


